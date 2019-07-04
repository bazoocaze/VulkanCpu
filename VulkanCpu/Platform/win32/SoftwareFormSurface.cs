/*
MIT License

Copyright (c) 2019 Jose Ferreira (Bazoocaze)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VulkanCpu.Engines.SoftwareEngine;
using VulkanCpu.VulkanApi;
using VulkanCpu.VulkanApi.Internals;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.Platform.win32
{
	public class SoftwareFormSurface : BaseSoftwareSurface
	{
		/// <summary>Mode of presentation of the image on the form surface.</summary>
		private enum FormSurfacePresentMode
		{
			/// <summary>Present using the DrawUnscaled() method of the Graphics class.</summary>
			GraphicsDrawUnscaled,

			/// <summary>Present using a DibBitmap and BitBlt() Windows function.</summary>
			DibBitmapBitBlt
		}

		private readonly SoftwareInstance m_instance;
		private VkWin32SurfaceCreateInfoKHR m_createInfo;

		private VkSurfaceCapabilitiesKHR m_Capabilities;
		private List<VkSurfaceFormatKHR> m_Formats;
		private List<VkPresentModeKHR> m_PresentModes;
		private Form m_Form;
		private VkExtent2D m_CurrentSurfaceExtents;
		private DibBitmap m_BitmapRenderTarget;

		private FormSurfacePresentMode m_SurfacePresentMode;

		public SoftwareFormSurface(SoftwareInstance instance, VkWin32SurfaceCreateInfoKHR createInfo)
		{
			this.m_Formats = new List<VkSurfaceFormatKHR>() {
				VkSurfaceFormatKHR.Create(VkFormat.VK_FORMAT_B8G8R8A8_UNORM, VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR)
			};

			this.m_PresentModes = new List<VkPresentModeKHR>()
			{
				VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR
			};

			this.m_instance = instance;
			this.m_createInfo = createInfo;

			foreach (Form form in Application.OpenForms)
			{
				if (form.Handle == createInfo.hwnd)
				{
					m_Form = form;
				}
			}

			if (m_Form == null)
			{
				throw new ArgumentException(string.Format("Form not found for the handle informed on the field {0}.{1}", nameof(VkWin32SurfaceCreateInfoKHR), nameof(VkWin32SurfaceCreateInfoKHR.hwnd)));
			}

			var m_OriginalExtents = VkExtent2D.Create(GetWidth(), GetHeight());
			m_CurrentSurfaceExtents = m_OriginalExtents;

			this.m_Capabilities = new VkSurfaceCapabilitiesKHR();

			this.m_Capabilities.currentExtent = m_OriginalExtents;
			this.m_Capabilities.maxImageExtent = m_OriginalExtents;
			this.m_Capabilities.minImageExtent = m_OriginalExtents;

			this.m_Capabilities.minImageCount = 1;
			this.m_Capabilities.maxImageCount = 8;

			this.m_Capabilities.maxImageArrayLayers = 1;

			this.m_Capabilities.supportedTransforms = VkSurfaceTransformFlagBitsKHR.VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR;
			this.m_Capabilities.currentTransform = VkSurfaceTransformFlagBitsKHR.VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR;

			this.m_Capabilities.supportedCompositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;

			this.m_Capabilities.supportedUsageFlags = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;

			this.m_SurfacePresentMode = FormSurfacePresentMode.DibBitmapBitBlt;

			InternalResize();

			m_Form.Resize += Form_Resize;
		}

		public void Destroy()
		{
			m_BitmapRenderTarget?.Dispose();
			if (m_Form != null)
			{
				m_Form.Resize -= Form_Resize;
				m_Form = null;
			}
		}

		private void Form_Resize(object sender, EventArgs e)
		{
			m_CurrentSurfaceExtents = VkExtent2D.Create(GetWidth(), GetHeight());
			InternalResize();
		}

		private int GetWidth()
		{
			return GetFormData(() => m_Form.ClientSize.Width);
		}

		private int GetHeight()
		{
			return GetFormData(() => m_Form.ClientSize.Height);
		}

		private T GetFormData<T>(Func<T> getData)
		{
			Form form = m_Form;
			if (form.InvokeRequired)
				return (T)form.Invoke(getData);
			return getData.Invoke();
		}

		private void InternalResize()
		{
			this.m_BitmapRenderTarget?.Dispose();
			this.m_BitmapRenderTarget = new DibBitmap(m_CurrentSurfaceExtents.width, m_CurrentSurfaceExtents.height);
		}

		public VkResult GetCapabilities(out VkSurfaceCapabilitiesKHR capabilities)
		{
			capabilities = m_Capabilities;
			capabilities.currentExtent = m_CurrentSurfaceExtents;
			capabilities.maxImageExtent = capabilities.currentExtent;
			capabilities.minImageExtent = capabilities.currentExtent;
			return VkResult.VK_SUCCESS;
		}

		public VkResult GetFormats(ref int formatCount, VkSurfaceFormatKHR[] formats)
		{
			return VkArrayUtil.CopyToList(m_Formats, formats, ref formatCount);
		}

		public VkResult GetPresentModes(ref int presentModeCount, VkPresentModeKHR[] presentModes)
		{
			return VkArrayUtil.CopyToList(m_PresentModes, presentModes, ref presentModeCount);
		}

		public VkResult PresentImage(SoftwareImage image, VkExtent2D imageExtent)
		{
			var form = m_Form;

			if (form == null || form.IsDisposed)
				return VkResult.VK_ERROR_DEVICE_LOST;

			VkResult result;

			if ((imageExtent.width != m_CurrentSurfaceExtents.width) || (imageExtent.height != m_CurrentSurfaceExtents.height))
			{
				InternalDrawUnscaled(image, form);
				return VkResult.VK_SUBOPTIMAL_KHR;
			}

			/*
			if ((imageExtent.width < m_CurrentExtents.width) || (imageExtent.height < m_CurrentExtents.height))
			{
				return VkResult.VK_ERROR_OUT_OF_DATE_KHR;
			} */

			if (form.InvokeRequired)
			{
				result = (VkResult)form.Invoke((Func<VkResult>)(() =>
				{
					return InternalPresent(image, form);
				}));
			}
			else
			{
				result = InternalPresent(image, form);
			}

			if ((imageExtent.width != m_CurrentSurfaceExtents.width) || (imageExtent.height != m_CurrentSurfaceExtents.height))
			{
				if (result == VkResult.VK_SUCCESS)
				{
					result = VkResult.VK_SUBOPTIMAL_KHR;
				}
			}

			return result;
		}

		private VkResult InternalPresent(SoftwareImage image, Form form)
		{
			switch (m_SurfacePresentMode)
			{
				case FormSurfacePresentMode.GraphicsDrawUnscaled:
					return InternalDrawUnscaled(image, form);

				case FormSurfacePresentMode.DibBitmapBitBlt:
					return InternalPresentDibBitmap(image, form);

				default:
					throw new NotImplementedException();
			}
		}

		private VkResult InternalPresentDibBitmap(SoftwareImage image, Form form)
		{
			m_BitmapRenderTarget.WritePixels(image.m_imageData, 0);
			using (Graphics target = form.CreateGraphics())
			{
				m_BitmapRenderTarget.DrawTo(target);
				return VkResult.VK_SUCCESS;
			}
		}

		public VkResult InternalDrawUnscaled(SoftwareImage source, Control destination)
		{
			VkResult result = VkResult.VK_SUCCESS;
			PixelFormat pixelFormat = PixelFormat.Format32bppRgb;
			int sourceWidth = source.GetWidth();
			int sourceHeight = source.GetHeight();
			int pixelSize = 4;

			using (Bitmap bitmap = new Bitmap(sourceWidth, sourceHeight, pixelFormat))
			{
				Rectangle rect = new Rectangle(0, 0, sourceWidth, sourceHeight);

				var bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, pixelFormat);

				if (bitmapData.Stride == (bitmapData.Width * pixelSize))
				{
					int arrayElements = bitmapData.Width * bitmapData.Height;
					Marshal.Copy(source.m_imageData, 0, bitmapData.Scan0, arrayElements);
				}
				else
				{
					for (int y = 0; y < sourceHeight; y++)
					{
						int sourceIndex = y * sourceWidth;
						int destIndex = y * bitmapData.Stride;
						Marshal.Copy(source.m_imageData, sourceIndex, bitmapData.Scan0 + destIndex, sourceWidth);
					}
				}

				bitmap.UnlockBits(bitmapData);

				using (Graphics target = destination.CreateGraphics())
				{
					target.DrawImageUnscaled(bitmap, 0, 0);
					target.Flush();
				}

				return result;
			}
		}

	}
}
