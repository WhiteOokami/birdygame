using UnityEngine;

namespace FunkyCode
{
	[System.Serializable]
	public class Lighting2DMaterials
	{
		private LightingMaterial occlusionEdge = null;
		private LightingMaterial occlusionBlur = null;

		private LightingMaterial additive = null;

		private LightingMaterial pointLight = null;
		private LightingMaterial spriteLight = null;
		private LightingMaterial freeFormLight = null;
		private LightingMaterial freeFormLightEdge = null;

		private LightingMaterial pointOcclusion = null;
		private LightingMaterial lightOcclusion = null;
		private LightingMaterial freeformOcclusion = null;

		private LightingMaterial maskBlurVertical = null;
		private LightingMaterial maskBlurHorizontal = null;

		private LightingMaterial multiplyHDR = null;
		private LightingMaterial alphablend = null;
		private LightingMaterial lightSprite = null;

		public Lighting2DMaterial.Mask mask = new Lighting2DMaterial.Mask();
		public Lighting2DMaterial.BumpMask bumpMask = new Lighting2DMaterial.BumpMask();
		public Lighting2DMaterial.Shadow shadow = new Lighting2DMaterial.Shadow();
		public Lighting2DMaterial.Room room = new Lighting2DMaterial.Room();

		public LightingSettings.HDR hdr = LightingSettings.HDR.Half;
		private bool initialized = false;

		public bool Initialize(LightingSettings.HDR allowHDR)
		{
			if (initialized)
			{
				if (allowHDR == hdr)
				{
					return(false);
				}
			}

			hdr = allowHDR;

			Reset();

			mask.Reset();
			shadow.Reset();
			room.Reset();
			bumpMask.Reset();

			initialized = true;

			mask.Initialize();
			shadow.Initialize();
			room.Initialize();
			bumpMask.Initialize();

			GetAdditive();

			GetSpriteLight();
			GetFreeFormLight();
			GetFreeFormEdgeLight();
			
			GetOcclusionBlur();
			GetOcclusionEdge();

			return(true);
		}

		public void Reset()
		{
			// is it the best way?
			initialized = false;

			maskBlurVertical = null;
			maskBlurHorizontal = null;

			occlusionEdge = null;
			occlusionBlur = null;

			additive = null;
			multiplyHDR = null;
			alphablend = null;

			pointOcclusion = null;
			lightOcclusion = null;
			freeformOcclusion = null;

			lightSprite = null;

			spriteLight = null;
			pointLight = null;
			freeFormLight = null;
			freeFormLightEdge = null;
		}
		
		public Material GetLightSprite()
		{
			if (lightSprite == null || lightSprite.Get() == null)
			{
				lightSprite = LightingMaterial.Load("Light2D/Internal/LightSprite");
			}

			return(lightSprite.Get());
		}

		public Material GetPointLight()
		{
			if (pointLight == null || pointLight.Get() == null)
			{
				pointLight = LightingMaterial.Load("Light2D/Internal/Light/PointLight");
			}

			return(pointLight.Get());
		}

		public Material GetMaskBlurVertical()
		{
			if (maskBlurVertical == null || maskBlurVertical.Get() == null)
			{
				maskBlurVertical = LightingMaterial.Load("Light2D/Internal/BlurVertical");
			}

			return(maskBlurVertical.Get());
		}

		public Material GetMaskBlurHorizontal()
		{
			if (maskBlurHorizontal == null || maskBlurHorizontal.Get() == null)
			{
				maskBlurHorizontal = LightingMaterial.Load("Light2D/Internal/BlurHorizontal");
			}

			return(maskBlurHorizontal.Get());
		}
			
		public Material GetSpriteLight()
		{
			if (spriteLight == null || spriteLight.Get() == null)
			{
				spriteLight = LightingMaterial.Load("Light2D/Internal/Light/SpriteLight");
			}

			return(spriteLight.Get());
		}

		public Material GetFreeFormLight()
		{
			if (freeFormLight == null || freeFormLight.Get() == null)
			{
				freeFormLight = LightingMaterial.Load("Light2D/Internal/Light/FreeFormLight");
			}

			return(freeFormLight.Get());
		}

		public Material GetFreeFormEdgeLight()
		{
			if (freeFormLightEdge == null || freeFormLightEdge.Get() == null)
			{
				freeFormLightEdge = LightingMaterial.Load("Light2D/Internal/Light/FreeFormFalloff");
			}
		
			return(freeFormLightEdge.Get());
		}

		public Material GetLightOcclusion()
		{
			if (lightOcclusion == null || lightOcclusion.Get() == null)
			{
				lightOcclusion = LightingMaterial.Load("Light2D/Internal/Light/SpriteLightOcclusion");
			}

			return(lightOcclusion.Get());
		}

		public Material GetPointOcclusion()
		{
			if (pointOcclusion == null || pointOcclusion.Get() == null)
			{
				pointOcclusion = LightingMaterial.Load("Light2D/Internal/Light/PointOcclusion");
			}

			return(pointOcclusion.Get());
		}

		public Material GetFreeFormOcclusion()
		{
			if (freeformOcclusion == null || freeformOcclusion.Get() == null)
			{
				freeformOcclusion = LightingMaterial.Load("Light2D/Internal/Light/FreeFormOcclusion");
			}

			return(freeformOcclusion.Get());
		}
		
		public Material GetAdditive()
		{
			if (additive == null || additive.Get() == null)
			{
				additive = LightingMaterial.Load("Light2D/Internal/Additive");
			}

			return(additive.Get());
		}

		public Material GetMultiplyHDR()
		{
			if (multiplyHDR == null || multiplyHDR.Get() == null)
			{
				if (hdr != LightingSettings.HDR.Off)
				{
					multiplyHDR = LightingMaterial.Load("Light2D/Internal/Multiply HDR");
				}
					else
				{
					multiplyHDR = LightingMaterial.Load("Light2D/Internal/Multiply");
				}
			}
			return(multiplyHDR.Get());
		}

		public Material GetAlphaBlend()
		{
			if (alphablend == null || alphablend.Get() == null)
			{
				alphablend = LightingMaterial.Load("Light2D/Internal/AlphaBlended");

				alphablend.SetTexture("textures/white");
			}

			return(alphablend.Get());
		}

		public Material GetOcclusionEdge()
		{
			if (occlusionEdge == null || occlusionEdge.Get() == null)
			{
				if (hdr != LightingSettings.HDR.Off)
				{
					occlusionEdge = LightingMaterial.Load("Light2D/Internal/Multiply HDR");
				}
					else
				{
					occlusionEdge = LightingMaterial.Load("Light2D/Internal/Multiply");
				}
				
				occlusionEdge.SetTexture("textures/occlusionedge");
			}

			return(occlusionEdge.Get());
		}

		public Material GetOcclusionBlur()
		{
			if (occlusionBlur == null || occlusionBlur.Get() == null)
			{
				if (hdr != LightingSettings.HDR.Off)
				{
					occlusionBlur = LightingMaterial.Load("Light2D/Internal/Multiply HDR");
				}
					else
				{
					occlusionBlur = LightingMaterial.Load("Light2D/Internal/Multiply");
				}
				
				occlusionBlur.SetTexture("textures/occlussionblur");
			}
			return(occlusionBlur.Get());
		}
	}
}