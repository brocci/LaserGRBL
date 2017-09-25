﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;

namespace LaserGRBL.RasterToGcode
{
	public partial class RasterToLaserForm : Form
	{
		Core.RasterToGcode.PreviewGenerator PG;
		bool preventClose;

		bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);
	
		private RasterToLaserForm(GrblCore core, string filename)
		{
			InitializeComponent();

			BackColor = ColorScheme.FormBackColor;
			GbConversionTool.ForeColor = GbLineToLineOptions.ForeColor = GbParameters.ForeColor = GbVectorizeOptions.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			PG = new Core.RasterToGcode.PreviewGenerator(core, PbConverted, filename);
			PbOriginal.Image = PG.OriginalImage;
			LblGrayscale.Visible = CbMode.Visible = !PG.IsGrayScale;
			
			CbResize.SuspendLayout();
			CbResize.AddItem(InterpolationMode.HighQualityBicubic);
			CbResize.AddItem(InterpolationMode.NearestNeighbor);
			CbResize.ResumeLayout();

			CbDither.SuspendLayout();
			foreach (Core.RasterToGcode.ImageTransform.DitheringMode formula in Enum.GetValues(typeof(Core.RasterToGcode.ImageTransform.DitheringMode)))
				CbDither.Items.Add(formula);
			CbDither.SelectedIndex = 0;
			CbDither.ResumeLayout();
			CbDither.SuspendLayout();

			CbMode.SuspendLayout();
			foreach (Core.RasterToGcode.ImageTransform.Formula formula in Enum.GetValues(typeof(Core.RasterToGcode.ImageTransform.Formula)))
				CbMode.AddItem(formula);
			CbMode.SelectedIndex = 0;
			CbMode.ResumeLayout();
			CbDirections.SuspendLayout();

			foreach (Core.RasterToGcode.ConversionTool.EngravingDirection direction in Enum.GetValues(typeof(Core.RasterToGcode.ConversionTool.EngravingDirection)))
				if (direction != Core.RasterToGcode.ConversionTool.EngravingDirection.None)
					CbDirections.AddItem(direction);
			CbDirections.SelectedIndex = 0;
			CbDirections.ResumeLayout();

			CbFillingDirection.SuspendLayout();
			foreach (Core.RasterToGcode.ConversionTool.EngravingDirection direction in Enum.GetValues(typeof(Core.RasterToGcode.ConversionTool.EngravingDirection)))
				CbFillingDirection.AddItem(direction);
			CbFillingDirection.SelectedIndex = 0;
			CbFillingDirection.ResumeLayout();

			RbLineToLineTracing.Visible = supportPWM;

			LoadSettings();
			RefreshVE();
		}
		
		//void OnPreviewBegin()
		//{
		//	preventClose = true;
				
		//	if (InvokeRequired)
		//	{
		//		Invoke(new Core.RasterToGcode.PreviewGenerator.PreviewBeginDlg(OnPreviewBegin));
		//	}
		//	else
		//	{
		//		WT.Enabled = true;
		//		BtnCreate.Enabled = false;				
		//	}
		//}
		//void OnPreviewReady(Image img)
		//{
		//	if (InvokeRequired)
		//	{
		//		Invoke(new Core.RasterToGcode.PreviewGenerator.PreviewReadyDlg(OnPreviewReady), img);
		//	}
		//	else
		//	{
		//		Image old = PbConverted.Image;
		//		PbOriginal.Image = PG.Original;
		//		PbConverted.Image = img.Clone() as Image;
		//		if (old != null)
		//			old.Dispose();
		//		WT.Enabled = false;
		//		WB.Visible = false;
		//		WB.Running = false;
		//		BtnCreate.Enabled = true;
		//		preventClose = false;
		//	}
		//}

		//void OnGenerationComplete(Exception ex)
		//{
		//	if (InvokeRequired)
		//	{
		//		BeginInvoke(new Core.RasterToGcode.PreviewGenerator.GenerationCompleteDlg(OnGenerationComplete), ex);
		//	}
		//	else
		//	{
		//		Cursor = Cursors.Default;
		//		if (ex != null)
		//			System.Windows.Forms.MessageBox.Show(ex.Message);
		//		preventClose = false;
		//		WT.Enabled = false;
		//		PG.Dispose();
		//		Close();
		//	}
		//}
		
		void WTTick(object sender, EventArgs e)
		{
			WT.Enabled = false;
			WB.Visible = true;
			WB.Running = true;
		}
		
		internal static void CreateAndShowDialog(GrblCore core, string filename, Form parent)
		{
			using (RasterToLaserForm f = new RasterToLaserForm(core, filename))
				f.ShowDialog(parent);
		}

		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		void BtnCreateClick(object sender, EventArgs e)
		{
			using (ConvertSizeAndOptionForm f = new ConvertSizeAndOptionForm())
			{
				f.ShowDialog(PG);
				if (f.DialogResult == DialogResult.OK)
				{
					preventClose = true;
					Cursor = Cursors.WaitCursor;
					SuspendLayout();
					TCOriginalPreview.SelectedIndex = 0;
					FlipControl.Enabled = false;
					BtnCreate.Enabled = false;
					WB.Visible = true;
					WB.Running = true;
					ResumeLayout();
			
					StoreSettings();
		
					//Core.RasterToGcode.PreviewGenerator targetProcessor = PG.Clone() as Core.RasterToGcode.PreviewGenerator;
					//PG.GenerateGCode();
				}
			}
		}

		private void StoreSettings()
		{
//			Settings.SetObject("GrayScaleConversion.RasterConversionTool", RbLineToLineTracing.Checked ? Core.RasterToGcode.PreviewGenerator.Tool.Line2Line : RbDithering.Checked ? Core.RasterToGcode.PreviewGenerator.Tool.Dithering : Core.RasterToGcode.PreviewGenerator.Tool.Vectorize);
			
//			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Direction", (Core.RasterToGcode.ConversionTool.EngravingDirection)CbDirections.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Quality", UDQuality.Value);
//			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Preview", CbLinePreview.Checked);

//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", CbSpotRemoval.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", UDSpotRemoval.Value);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", CbSmoothing.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", UDSmoothing.Value);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", CbOptimize.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", UDOptimize.Value);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.DownSample.Enabled", CbDownSample.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.DownSample.Value", UDDownSample.Value);
////			Settings.SetObject("GrayScaleConversion.VectorizeOptions.ShowDots.Enabled", CbShowDots.Checked);
////			Settings.SetObject("GrayScaleConversion.VectorizeOptions.ShowImage.Enabled", CbShowImage.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.FillingDirection", (Core.RasterToGcode.ConversionTool.EngravingDirection)CbFillingDirection.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.FillingQuality", UDFillingQuality.Value);

//			Settings.SetObject("GrayScaleConversion.DitheringOptions.DitheringMode", (Core.RasterToGcode.ImageTransform.DitheringMode)CbDither.SelectedItem);

//			Settings.SetObject("GrayScaleConversion.Parameters.Interpolation", (InterpolationMode)CbResize.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.Parameters.Mode", (Core.RasterToGcode.ImageTransform.Formula)CbMode.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.Parameters.R", TBRed.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.G", TBGreen.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.B", TBBlue.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.Brightness", TbBright.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.Contrast", TbContrast.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Enabled", CbThreshold.Checked);
//			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Value", TbThreshold.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.WhiteClip", TBWhiteClip.Value);

//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", PG.BorderSpeed);
//			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Mark", PG.MarkSpeed);
//			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Travel", PG.TravelSpeed);

//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", PG.LaserOn);
//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", PG.LaserOff);
//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", PG.MinPower);
//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", PG.MaxPower);

//			Settings.SetObject("GrayScaleConversion.Gcode.Offset.X", PG.TargetOffset.X);
//			Settings.SetObject("GrayScaleConversion.Gcode.Offset.Y", PG.TargetOffset.Y);
//			Settings.SetObject("GrayScaleConversion.Gcode.BiggestDimension", Math.Max(PG.TargetSize.Width, PG.TargetSize.Height));


//			Settings.Save(); // Saves settings in application configuration file
		}

		private void LoadSettings()
		{
			//if ((PG.SelectedTool = (Core.RasterToGcode.PreviewGenerator.Tool)Settings.GetObject("GrayScaleConversion.RasterConversionTool", Core.RasterToGcode.PreviewGenerator.Tool.Line2Line)) == Core.RasterToGcode.PreviewGenerator.Tool.Line2Line)
			//	RbLineToLineTracing.Checked = true;
			//else if ((PG.SelectedTool = (Core.RasterToGcode.PreviewGenerator.Tool)Settings.GetObject("GrayScaleConversion.RasterConversionTool", Core.RasterToGcode.PreviewGenerator.Tool.Line2Line)) == Core.RasterToGcode.PreviewGenerator.Tool.Dithering)
			//	RbDithering.Checked = true;
			//else
			//	RbVectorize.Checked = true;

			//CbDirections.SelectedItem = PG.LineDirection = (Core.RasterToGcode.ConversionTool.EngravingDirection)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Direction", Core.RasterToGcode.ConversionTool.EngravingDirection.Horizontal);
			//UDQuality.Value = (decimal)(PG.Quality = Convert.ToDouble(Settings.GetObject("GrayScaleConversion.Line2LineOptions.Quality", 3.0)));
			//CbLinePreview.Checked = PG.LinePreview = (bool)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Preview", false);

			//CbSpotRemoval.Checked = PG.UseSpotRemoval = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", false);
			//UDSpotRemoval.Value = PG.SpotRemoval = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", 2.0m);
			//CbSmoothing.Checked = PG.UseSmoothing = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", false);
			//UDSmoothing.Value = PG.Smoothing = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", 1.0m);
			//CbOptimize.Checked = PG.UseOptimize = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", false);
			//UDOptimize.Value = PG.Optimize = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", 0.2m);
			//CbDownSample.Checked = PG.UseDownSampling = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.DownSample.Enabled", false);
			//UDDownSample.Value = PG.DownSampling = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.DownSample.Value", 2.0m);

			////CbShowDots.Checked = IP.ShowDots = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.ShowDots.Enabled", false);
			////CbShowImage.Checked = IP.ShowImage = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.ShowImage.Enabled", true);
			//CbFillingDirection.SelectedItem = PG.FillingDirection = (Core.RasterToGcode.ConversionTool.EngravingDirection)Settings.GetObject("GrayScaleConversion.VectorizeOptions.FillingDirection", Core.RasterToGcode.ConversionTool.EngravingDirection.None);
			//UDFillingQuality.Value = (decimal)(PG.FillingQuality = Convert.ToDouble(Settings.GetObject("GrayScaleConversion.VectorizeOptions.FillingQuality", 3.0)));

			//CbResize.SelectedItem = PG.Interpolation = (InterpolationMode)Settings.GetObject("GrayScaleConversion.Parameters.Interpolation", InterpolationMode.HighQualityBicubic);
			//CbMode.SelectedItem = PG.Formula = (Core.RasterToGcode.ImageTransform.Formula)Settings.GetObject("GrayScaleConversion.Parameters.Mode", Core.RasterToGcode.ImageTransform.Formula.SimpleAverage);
			//TBRed.Value = PG.Red = (int)Settings.GetObject("GrayScaleConversion.Parameters.R", 100);
			//TBGreen.Value = PG.Green = (int)Settings.GetObject("GrayScaleConversion.Parameters.G", 100);
			//TBBlue.Value = PG.Blue = (int)Settings.GetObject("GrayScaleConversion.Parameters.B", 100);
			//TbBright.Value = PG.Brightness = (int)Settings.GetObject("GrayScaleConversion.Parameters.Brightness", 100);
			//TbContrast.Value = PG.Contrast = (int)Settings.GetObject("GrayScaleConversion.Parameters.Contrast", 100);
			//CbThreshold.Checked = PG.UseThreshold = (bool)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Enabled", false);
			//TbThreshold.Value = PG.Threshold = (int)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Value", 50);
			//TBWhiteClip.Value = PG.WhiteClip = (int)Settings.GetObject("GrayScaleConversion.Parameters.WhiteClip", 5);

			//CbDither.SelectedItem = (Core.RasterToGcode.ImageTransform.DitheringMode)Settings.GetObject("GrayScaleConversion.DitheringOptions.DitheringMode", Core.RasterToGcode.ImageTransform.DitheringMode.FloydSteinberg);

			//if (RbLineToLineTracing.Checked && !supportPWM)
			//	RbDithering.Checked = true;
		}

		void OnRGBCBDoubleClick(object sender, EventArgs e)
		{((UserControls.ColorSlider)sender).Value = 100;}

		void OnThresholdDoubleClick(object sender, EventArgs e)
		{((UserControls.ColorSlider)sender).Value = 50;}

		private void CbMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.Formula = (Core.RasterToGcode.ImageTransform.Formula)CbMode.SelectedItem;

				SuspendLayout();
				TBRed.Visible = TBGreen.Visible = TBBlue.Visible = (PG.Formula == Core.RasterToGcode.ImageTransform.Formula.Custom && !PG.IsGrayScale);
				LblRed.Visible = LblGreen.Visible = LblBlue.Visible = (PG.Formula == Core.RasterToGcode.ImageTransform.Formula.Custom && !PG.IsGrayScale);
				ResumeLayout();
			}
		}

		private void TBRed_ValueChanged(object sender, EventArgs e)
		{if (PG != null) PG.Red = TBRed.Value; }

		private void TBGreen_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Green = TBGreen.Value; }

		private void TBBlue_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Blue = TBBlue.Value; }

		private void TbBright_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Brightness = TbBright.Value; }

		private void TbContrast_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Contrast = TbContrast.Value; }

		private void CbThreshold_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.UseThreshold = CbThreshold.Checked;
				RefreshVE();
			}
		}

		private void RefreshVE()
		{
			GbVectorizeOptions.Visible = RbVectorize.Checked;
			GbLineToLineOptions.Visible = RbLineToLineTracing.Checked || RbDithering.Checked;
			GbLineToLineOptions.Text = RbLineToLineTracing.Checked ? "Line To Line Options" : "Dithering Options";

			CbThreshold.Visible = !RbDithering.Checked;
			TbThreshold.Visible = !RbDithering.Checked && CbThreshold.Checked;

			LblDitherMode.Visible = CbDither.Visible = RbDithering.Checked;
		}

		private void TbThreshold_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Threshold = TbThreshold.Value; }

		private void RbLineToLineTracing_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				if (RbLineToLineTracing.Checked)
					PG.SelectedTool = Core.RasterToGcode.PreviewGenerator.Tool.Line2Line;
				RefreshVE();
			}
		}
		
		private void RbVectorize_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				if (RbVectorize.Checked)
					PG.SelectedTool = Core.RasterToGcode.PreviewGenerator.Tool.Vectorize;
				RefreshVE();
			}
		}

		private void UDQuality_ValueChanged(object sender, EventArgs e)
		{ if (PG != null)  PG.Quality = (double)UDQuality.Value;  }

		private void CbLinePreview_CheckedChanged(object sender, EventArgs e)
		{ if (PG != null) PG.LinePreview = CbLinePreview.Checked; }

		private void UDSpotRemoval_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.SpotRemoval = (int)UDSpotRemoval.Value; }

		private void CbSpotRemoval_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null)
				PG.UseSpotRemoval = CbSpotRemoval.Checked;
			UDSpotRemoval.Enabled = CbSpotRemoval.Checked;
		}

		private void UDSmoothing_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Smoothing = UDSmoothing.Value; }

		private void CbSmoothing_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null) PG.UseSmoothing = CbSmoothing.Checked;
			UDSmoothing.Enabled = CbSmoothing.Checked;
		}

		private void UDOptimize_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.Optimize = UDOptimize.Value; }

		private void CbOptimize_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null) PG.UseOptimize = CbOptimize.Checked;
			UDOptimize.Enabled = CbOptimize.Checked;
		}

		private void RasterToLaserForm_Load(object sender, EventArgs e)
		{ if (PG != null) PG.Resume(); }
		
		void RasterToLaserFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (preventClose)
			{
				e.Cancel = true;
			}
			else
			{
				//Core.RasterToGcode.PreviewGenerator.PreviewReady -= OnPreviewReady;
				//Core.RasterToGcode.PreviewGenerator.PreviewBegin -= OnPreviewBegin;
				//Core.RasterToGcode.PreviewGenerator.GenerationComplete -= OnGenerationComplete;
				if (PG != null) PG.Dispose();
			}
		}

		void CbDirectionsSelectedIndexChanged(object sender, EventArgs e)
		{ if (PG != null)PG.LineDirection = (Core.RasterToGcode.ConversionTool.EngravingDirection)CbDirections.SelectedItem; }

		void CbResizeSelectedIndexChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.Interpolation = (InterpolationMode)CbResize.SelectedItem;
				PbOriginal.Image = PG.Original;
			}
		}
		void BtRotateCWClick(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.RotateCW();
				PbOriginal.Image = PG.Original;
			}
		}
		void BtRotateCCWClick(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.RotateCCW();
				PbOriginal.Image = PG.Original;
			}
		}
		void BtFlipHClick(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.FlipH();
				PbOriginal.Image = PG.Original;
			}
		}
		void BtFlipVClick(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.FlipV();
				PbOriginal.Image = PG.Original;
			}
		}
		
		void BtnRevertClick(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.Revert();
				PbOriginal.Image = PG.Original;
			}
		}

		private void CbFillingDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.FillingDirection = (Core.RasterToGcode.ConversionTool.EngravingDirection)CbFillingDirection.SelectedItem;
				BtnFillingQualityInfo.Visible = LblFillingLineLbl.Visible = LblFillingQuality.Visible = UDFillingQuality.Visible = ((Core.RasterToGcode.ConversionTool.EngravingDirection)CbFillingDirection.SelectedItem != Core.RasterToGcode.ConversionTool.EngravingDirection.None);
			}
		}

		private void UDFillingQuality_ValueChanged(object sender, EventArgs e)
		{
			if (PG != null)
				PG.FillingQuality = (double)UDFillingQuality.Value;
		}
		
		
		bool isDrag = false;
		Rectangle imageRectangle;
  		Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
		Point sP;
		Point eP;
	  	
		void PbConvertedMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Left && Cropping)
			{
				int left = (PbConverted.Width - PbConverted.Image.Width) / 2;
				int top = (PbConverted.Height - PbConverted.Image.Height) / 2;
				int right = PbConverted.Width - left;
				int bottom = PbConverted.Height - top;

				imageRectangle = new Rectangle(left, top, PbConverted.Image.Width, PbConverted.Image.Height);
				
				if ((e.X >= left && e.Y >= top) && (e.X <= right && e.Y <= bottom))
				{
					isDrag = true;
					sP = e.Location;
					eP = e.Location;
				}
			}
	
		}
		void PbConvertedMouseMove(object sender, MouseEventArgs e)
		{
			if (isDrag)
			{
				//erase old rectangle
				ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Dashed);

				eP = e.Location;
				
				//limit eP to image rectangle
				int left = (PbConverted.Width - PbConverted.Image.Width) / 2;
				int top = (PbConverted.Height - PbConverted.Image.Height) / 2;
				int right = PbConverted.Width - left;
				int bottom = PbConverted.Height - top;
				eP.X = Math.Min(Math.Max(eP.X, left), right);
				eP.Y = Math.Min(Math.Max(eP.Y, top), bottom);
				
				theRectangle = new Rectangle(PbConverted.PointToScreen(sP), new Size(eP.X-sP.X, eP.Y-sP.Y));
		
				// Draw the new rectangle by calling DrawReversibleFrame
				ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Dashed);
			}
		}
		
		void PbConvertedMouseUp(object sender, MouseEventArgs e)
		{
			// If the MouseUp event occurs, the user is not dragging.
			if (isDrag)
			{
				isDrag = false;
				
				//erase old rectangle
				ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Dashed);
				

				int left = (PbConverted.Width - PbConverted.Image.Width) / 2;
				int top = (PbConverted.Height - PbConverted.Image.Height) / 2;
				
				Rectangle CropRect = new Rectangle(Math.Min(sP.X, eP.X) - left,
			                                         Math.Min(sP.Y, eP.Y) - top,
			                                         Math.Abs(eP.X-sP.X),
			                                         Math.Abs(eP.Y-sP.Y));
				
				//Rectangle CropRect = new Rectangle(p.X-left, p.Y-top, orientedRect.Width, orientedRect.Height);
				
				PG.CropImage(CropRect, PbConverted.Image.Size);
				
				PbOriginal.Image = PG.Original;
				
				// Reset the rectangle.
				theRectangle = new Rectangle(0, 0, 0, 0);
				Cropping = false;
				Cursor.Clip = new Rectangle();
				UpdateCropping();
			}
		}
		
		bool Cropping;
		void BtnCropClick(object sender, EventArgs e)
		{
			Cropping = !Cropping;
			UpdateCropping();
		}
		
		void UpdateCropping()
		{
			if (Cropping)
				BtnCrop.BackColor = Color.Orange;
			else
				BtnCrop.BackColor = DefaultBackColor;
		}
		void BtnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		private void RbDithering_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				if (RbDithering.Checked)
					PG.SelectedTool = Core.RasterToGcode.PreviewGenerator.Tool.Dithering;
				RefreshVE();
			}
		}

		private void CbDownSample_CheckedChanged(object sender, EventArgs e)
		{
			if (PG != null)
			{
				PG.UseDownSampling = CbDownSample.Checked;
				UDDownSample.Enabled = CbDownSample.Checked;
			}
		}

		private void UDDownSample_ValueChanged(object sender, EventArgs e)
		{
			if (PG != null)
				PG.DownSampling = UDDownSample.Value;
		}

		private void PbConverted_Resize(object sender, EventArgs e)
		{
			if (PG != null)
				PG.FormResize(PbConverted.Size);
		}

		private void CbDither_SelectedIndexChanged(object sender, EventArgs e)
		{ if (PG != null) PG.DitheringMode = (Core.RasterToGcode.ImageTransform.DitheringMode)CbDither.SelectedItem; }

		private void BtnQualityInfo_Click(object sender, EventArgs e)
		{
			UDQuality.Value = (decimal)ResolutionHelperForm.CreateAndShowDialog((double)UDQuality.Value);
			//System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}

		private void BtnFillingQualityInfo_Click(object sender, EventArgs e)
		{
			UDFillingQuality.Value = (decimal)ResolutionHelperForm.CreateAndShowDialog((double)UDFillingQuality.Value);
			//System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}

		private void TBWhiteClip_ValueChanged(object sender, EventArgs e)
		{ if (PG != null) PG.WhiteClip = TBWhiteClip.Value; }

		private void TBWhiteClip_MouseDown(object sender, MouseEventArgs e)
		{ if (PG != null) PG.Demo = true; }

		private void TBWhiteClip_MouseUp(object sender, MouseEventArgs e)
		{ if (PG != null) PG.Demo = false; }

	}
}