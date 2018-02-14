using Polynano.Common;
using Polynano.IO;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polynano.UI
{
    /// <summary>
    /// Responsible for the Window and the user interface
    /// </summary>
    internal partial class GuiApplicationForm : Form
    {
        /// <summary>
        /// The Application to use
        /// </summary>
        private readonly Application _app;

        /// <summary>
        /// The viewer to display the model
        /// </summary>
        private readonly Viewer _viewer;

        /// <summary>
        /// The name of the file that was loaded
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The target number of faces of the model
        /// </summary>
        private int _targetVertexCount;

        /// <summary>
        /// whether we're reducing or increasing the number of faces
        /// </summary>
        private bool _reduce;

        /// <summary>
        /// The current Model (simplified)
        /// </summary>
        private MeshGeometryData _currentModel;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="app">Application instance to use</param>
        public GuiApplicationForm(Application app)
        {
            _app = app;
            _viewer = new Viewer
            {
                Parent = this
            };
            InitializeComponent();
            complexityTrackbar.Value = complexityTrackbar.Maximum;
        }

        public new void Dispose()
        {
            _viewer.Dispose();
            base.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            _viewer?.UpdateSize(leftPanel.Width, 0, ClientSize.Width - leftPanel.Width, ClientSize.Height);
            authorLabel.Location = new Point(authorLabel.Location.X, Height - 60 < leftPanel.Height ? leftPanel.Height + 60 : Height - 60);
        }

        private async void OnLoadButtonClicked(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            try
            {
                // if the file is smaller than 4MB do not show the loading window
                if (new FileInfo(dialog.FileName).Length < 2e+6)
                {
                    _app.Load(dialog.FileName);
                }
                else
                {
                    await ProvideLoadingFormForAction(p =>
                    {
                        _app.Load(dialog.FileName, p);
                        Thread.Sleep(700);
                    }, dialog.SafeFileName + " wird geladen...");
                }

                if (_app.OriginalModel.Faces.Length < 100000)
                {
                    _app.InitializeSimplifier();
                }
                else
                {
                    await ProvideLoadingFormForAction(p =>
                    {
                        _app.InitializeSimplifier();
                        Thread.Sleep(100);
                    }, "Geometrie wird verarbeitet...", ProgressBarStyle.Marquee);
                }

            }
            catch (ArgumentException)
            {
                MessageBox.Show(@"Die Datei konnte nicht geladen werden", @"Fehler");
                return;
            }

            _currentModel = _app.Simplifier.GetModel();
            _viewer.Transformation = VertexNormalizer.GetNormalizingMatrix(_currentModel.Vertices);
            _viewer.SetMesh(_currentModel);

            _fileName = dialog.SafeFileName;
            simplifyButton.Enabled = true;
            complexityTrackbar.Value = complexityTrackbar.Maximum;
            complexityTrackbar.Enabled = true;
            modelStatsTable.Visible = true;
            UpdateStatistics();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {

            if (_app.Simplifier.GetModel().Faces.Length == 0)
            {
                return;
            }

            var dialog = new SaveFileDialog
            {
                FileName = "Simplified_" + _fileName,
                AddExtension = true,
                DefaultExt = "ply"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // If the model is relativly small, do not show the progress window
            if (_currentModel.Faces.Length < 100000)
            {
                _app.Save(dialog.FileName);
            }
            else
            {
                // Show the progress window
                await ProvideLoadingFormForAction(p =>
                {
                    _app.Save(dialog.FileName, p);
                    // Wait 1/10 of a second for the progress bar animation to go to 100 %
                    Thread.Sleep(100);
                }, "Saving " + _fileName + " ...");
            }
            MessageBox.Show(Path.GetFileName(dialog.FileName) + @" wurde erfolgreich gespeichert", @"Erfolg");
        }

        /// <summary>
        /// Called by windows when the user clicked the simplify button
        /// </summary>
        private void OnSimplifyButtonClicked(object sender, EventArgs e)
        {
            if (_fileName == null)
            {
                return;
            }

            saveButton.Enabled = true;
            simplifyButton.Enabled = false;

            float percent = (complexityTrackbar.Value / 5f);
            _targetVertexCount = (int)Math.Floor((double)(percent * _app.OriginalModel.Vertices.Length) / 100);
            _reduce = _targetVertexCount <= _currentModel.ActiveVertices.Length;

            if (_app.OriginalModel.Faces.Length > 70000)
            {
                InstantlySimplify();
            }
            else
            {
                simplificationTimer.Interval = 80;
                simplificationTimer.Enabled = true;
            }
        }

        private async void InstantlySimplify()
        {
              await ProvideLoadingFormForAction(p =>
              {
                  int count;
                  if (_reduce)
                  {
                      count = Math.Min(_currentModel.ActiveVertices.Length - _targetVertexCount, _currentModel.ActiveVertices.Length);
                  }
                  else
                  {
                      count = _targetVertexCount - _currentModel.ActiveVertices.Length + 1;
                  }

                  int step = (int)(count * 0.05f);
                  int lastProgress = 0;
                  if (_reduce)
                  {
                      for (int i = 0; i < count; i++)
                      {
                          _app.Simplifier.SimplifyOneStep();
                          if (i > step)
                          {
                              p.Report(i * 100 / count);
                              lastProgress += step;
                          }
                      }
                  }
                  else
                  {
                      for (int i = 0; i < count; i++)
                      {
                          _app.Simplifier.RevertOneStep();
                          if (i > lastProgress)
                          {
                              p.Report(lastProgress * 100 / count);
                              lastProgress += 1000;
                          }
                      }
                  }
                  p.Report(100);
                  Thread.Sleep(700);
              }, "Bitte warten...");

            simplifyButton.Enabled = true;
            UpdateView();
            UpdateStatistics();
        }

        /// <summary>
        /// Called by windows on every timer tick
        /// </summary>
        private void OnSimplificationTimerTick(object sender, EventArgs e)
        {
            // simplify or revert the model by some small factor
            if (_reduce && _currentModel.ActiveVertices.Length <= _targetVertexCount
               || !_reduce && _currentModel.ActiveVertices.Length >= _targetVertexCount)
            {
                simplificationTimer.Enabled = false;
                simplifyButton.Enabled = true;
                UpdateStatistics();
                return;
            }

            var verticesToRemoveProTick = Math.Max((int)(_currentModel.ActiveVertices.Length * 0.09f), 1);
            var verticesOnBegin = _currentModel.ActiveVertices.Length;
            if (_reduce)
            {

                for (int i = 0; i < verticesToRemoveProTick; i++)
                {
                    _app.Simplifier.SimplifyOneStep();
                }
            }
            else
            {
                for (int i = 0; i < verticesToRemoveProTick; i++)
                {
                    _app.Simplifier.RevertOneStep();
                }
            }

            UpdateView();

            // Break if no changes were made
            if (_currentModel.ActiveVertices.Length == verticesOnBegin)
            {
                simplifyButton.Enabled = false;
                simplifyButton.Enabled = true;
                UpdateStatistics();
            }
        }

        private void UpdateStatistics()
        {
            statsSizeLabel.Text = GetSizeForModel(_currentModel.Faces.Length, _currentModel.ActiveVertices.Length);
            StatsFaceCountLabel.Text = _currentModel.Faces.Length.ToString();
            StatsVertexCountLabel.Text = _currentModel.ActiveVertices.Length.ToString();

            float percent = _currentModel.Faces.Length * 100f / _app.OriginalModel.Faces.Length;
            string p = percent.ToString();

            if (p.EndsWith(",00"))
            {
                p = p.Substring(p.Length - 3, 3);
            }

            statsComplexityLabel.Text = percent.ToString("n2") + "%";
        }

        private void UpdateView()
        {
            _currentModel = _app.Simplifier.GetModel();
            _viewer.UpdateMesh(_currentModel);
        }

        private static async Task ProvideLoadingFormForAction(Action<IProgress<int>> action, string title, ProgressBarStyle style = ProgressBarStyle.Continuous)
        {
            var loadingForm = new LoadingForm
            {
                StartPosition = FormStartPosition.CenterScreen,
                StatusText = title,
                Style = style
            };
            loadingForm.Show();
            var progressHandler = new Progress<int>(value => { loadingForm.StatusBar = value; });
            var progress = (IProgress<int>)progressHandler;
            await Task.Run(() => action(progress));
            loadingForm.Dispose();
        }

        private void OnViewFacesCheckbox_CheckedChanged(object sender, EventArgs e)
            => _viewer.DisplayFaces = viewFacesCheckbox.Checked;

        private void OnViewEdgesCheckbox_CheckedChanged(object sender, EventArgs e)
            => _viewer.DisplayEdges = viewEdgesCheckbox.Checked;

        private void OnViewVerticesCheckbox_CheckedChanged(object sender, EventArgs e)
            => _viewer.DisplayVertices = viewVerticesCheckbox.Checked;

        private void OnComplexityTrackbar_ValueChanged(object sender, EventArgs e)
        {
        }

        private string GetSizeForModel(int faceCount, int vertexCount)
        {
            float estimatedSize = (faceCount * 4 * sizeof(int))
                                  + (vertexCount * 3 * sizeof(float)) + 203; // 203 is the header size. Note that 203 is an estimate. 
            // in the header the number of faces and vertices is stored and depending on how big the number is, more characters will 
            // need to be stored therefore increasing the header size. It is possible to compute the exact number of bytes needed, however I don't care.

            estimatedSize /= 1048576; // convert bytes to mb
            return estimatedSize.ToString("0.##") + @" MB";
        }
    }
}
