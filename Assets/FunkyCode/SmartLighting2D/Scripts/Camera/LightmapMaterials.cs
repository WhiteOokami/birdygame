﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkyCode
{
	[System.Serializable]
	public class LightmapMaterials
	{
		public Material[] materials = new Material[1];

		public static void ClearMaterial(Material material)
		{
			// game
			material.SetTexture("_GameTexture1", null);
			material.SetVector("_GameRect1", new Vector4(0, 0, 0, 0));
			material.SetFloat("_GameRotation1", 0);

			material.SetTexture("_GameTexture2", null);
			material.SetVector("_GameRect2", new Vector4(0, 0, 0, 0));
			material.SetFloat("_GameRotation2", 0);

			material.SetTexture("_GameTexture3", null);
			material.SetVector("_GameRect3", new Vector4(0, 0, 0, 0));
			material.SetFloat("_GameRotation3", 0);

			material.SetTexture("_GameTexture4", null);
			material.SetVector("_GameRect4", new Vector4(0, 0, 0, 0));
			material.SetFloat("_GameRotation4", 0);

			// scene
			material.SetTexture("_SceneTexture1", null);
			material.SetVector("_SceneRect1", new Vector4(0, 0, 0, 0));
			material.SetFloat("_SceneRotation1", 0);

			material.SetTexture("_SceneTexture2", null);
			material.SetVector("_SceneRect2", new Vector4(0, 0, 0, 0));
			material.SetFloat("_SceneRotation2", 0);

			material.SetTexture("_SceneTexture3", null);
			material.SetVector("_SceneRect3", new Vector4(0, 0, 0, 0));
			material.SetFloat("_SceneRotation3", 0);

			material.SetTexture("_SceneTexture4", null);
			material.SetVector("_SceneRect4", new Vector4(0, 0, 0, 0));
			material.SetFloat("_SceneRotation4", 0);
		}

		public static void ResetShaders()
		{
			// game
			Shader.SetGlobalTexture("_GameTexture1", null);
			Shader.SetGlobalVector("_GameRect1", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_GameRotation1", 0);

			Shader.SetGlobalTexture("_GameTexture2", null);
			Shader.SetGlobalVector("_GameRect2", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_GameRotation2", 0);

			Shader.SetGlobalTexture("_GameTexture3", null);
			Shader.SetGlobalVector("_GameRect3", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_GameRotation3", 0);

			Shader.SetGlobalTexture("_GameTexture4", null);
			Shader.SetGlobalVector("_GameRect4", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_GameRotation4", 0);

			// scene
			Shader.SetGlobalTexture("_SceneTexture1", null);
			Shader.SetGlobalVector("_SceneRect1", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_SceneRotation1", 0);

			Shader.SetGlobalTexture("_SceneTexture2", null);
			Shader.SetGlobalVector("_SceneRect2", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_SceneRotation2", 0);

			Shader.SetGlobalTexture("_SceneTexture3", null);
			Shader.SetGlobalVector("_SceneRect3", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_SceneRotation3", 0);

			Shader.SetGlobalTexture("_SceneTexture4", null);
			Shader.SetGlobalVector("_SceneRect4", new Vector4(0, 0, 0, 0));
			Shader.SetGlobalFloat("_SceneRotation4", 0);
		}

		public static void SetShaders(bool isSceneView, int id, Camera camera, LightTexture lightTexture)
		{
			float ratio = (float)camera.pixelRect.width / camera.pixelRect.height;

			float x = camera.transform.position.x;
			float y = camera.transform.position.y;

			// z = width ; w = height
			float w = camera.orthographicSize * 2;
			float z = w * ratio;
		
			float rotation = camera.transform.eulerAngles.z * Mathf.Deg2Rad;

			Vector4 rect = new Vector4(x, y, z, w);

			if (lightTexture == null)
			{
				Debug.Log("light texture null");
				return;
			}

			bool gameView = !isSceneView;

			if (gameView)
			{
				switch(id)
				{
					case 1:
						Shader.SetGlobalTexture("_GameTexture1", lightTexture.renderTexture);
						Shader.SetGlobalVector("_GameRect1", rect);
						Shader.SetGlobalFloat("_GameRotation1", rotation);
					break;

					case 2:
						Shader.SetGlobalTexture("_GameTexture2", lightTexture.renderTexture);
						Shader.SetGlobalVector("_GameRect2", rect);
						Shader.SetGlobalFloat("_GameRotation2", rotation);
					break;

					case 3:
						Shader.SetGlobalTexture("_GameTexture3", lightTexture.renderTexture);
						Shader.SetGlobalVector("_GameRect3", rect);
						Shader.SetGlobalFloat("_GameRotation3", rotation);
					break;

					case 4:
						Shader.SetGlobalTexture("_GameTexture4", lightTexture.renderTexture);
						Shader.SetGlobalVector("_GameRect4", rect);
						Shader.SetGlobalFloat("_GameRotation4", rotation);
					break;
				}
			}
				else
			{
				switch(id)
				{
					case 1:
						Shader.SetGlobalTexture("_SceneTexture1", lightTexture.renderTexture);
						Shader.SetGlobalVector("_SceneRect1", rect);
						Shader.SetGlobalFloat("_SceneRotation1", rotation);
					break;

					case 2:
						Shader.SetGlobalTexture("_SceneTexture2", lightTexture.renderTexture);
						Shader.SetGlobalVector("_SceneRect2", rect);
						Shader.SetGlobalFloat("_SceneRotation2", rotation);
					break;

					case 3:
						Shader.SetGlobalTexture("_SceneTexture3", lightTexture.renderTexture);
						Shader.SetGlobalVector("_SceneRect3", rect);
						Shader.SetGlobalFloat("_SceneRotation3", rotation);
					break;

					case 4:
						Shader.SetGlobalTexture("_SceneTexture4", lightTexture.renderTexture);
						Shader.SetGlobalVector("_SceneRect4", rect);
						Shader.SetGlobalFloat("_SceneRotation4", rotation);

						Debug.Log("do" + lightTexture.renderTexture);
					break;
				}
			}
		}

		public static void SetDayLight()
		{
			float direction = -(Lighting2D.DayLightingSettings.direction - 180) * Mathf.Deg2Rad;
			float height = Lighting2D.DayLightingSettings.bumpMap.height;

			Shader.SetGlobalFloat("_Day_Direction", direction);
			Shader.SetGlobalFloat("_Day_Height", height);
		}

		public static void SetMaterial(int id, Material material, Camera camera, LightTexture lightTexture) {
			float ratio = (float)camera.pixelRect.width / camera.pixelRect.height;

			float x = camera.transform.position.x;
			float y = camera.transform.position.y;

			// z = size x ; w = size y
			float w = camera.orthographicSize * 2;
			float z = w * ratio;

			float rotation = camera.transform.eulerAngles.z * Mathf.Deg2Rad;

			Vector4 rect = new Vector4(x, y, z, w);

			switch(id) {
				case 1:
					material.SetTexture("_GameTexture1", lightTexture.renderTexture);
					material.SetVector("_GameRect1", rect);
					material.SetFloat("_GameRotation1", rotation);
				break;

				case 2:
					material.SetTexture("_GameTexture2", lightTexture.renderTexture);
					material.SetVector("_GameRect2", rect);
					material.SetFloat("_GameRotation2", rotation);
				break;

				// add 3 & 4
			}
			
		}

		public void Add(Material material) {
			foreach(Material m in  materials) {
				if (m == material) {
					Debug.Log("Lighting Manager 2D: Failed to add material (material already added!");
					return;
				}
			}

			for(int i = 0 ; i < materials.Length; i++) {
				if (materials[i] != null) {
					continue;
				}
				materials[i] = material;

				return;
			}

			System.Array.Resize(ref materials, materials.Length + 1);

			materials[materials.Length - 1] = material;
		}

		public void Remove(Material material) {
			for(int i = 0 ; i < materials.Length; i++) {
				if (materials[i] != material) {
					continue;
				}
				materials[i] = null;

				return;
			}

			Debug.LogWarning("Lighting Manager 2D: Removing material that does not exist");
		}
	}
}

		/*
		public static int GetFreeId() {
			Vector4 rect;

			rect = Shader.GetGlobalVector("_GameRect1");

			if (rect.z <= 0) {
				return(1);
			}

			rect = Shader.GetGlobalVector("_GameRect2");

			if (rect.z <= 0) {
				return(2);
			}

			rect = Shader.GetGlobalVector("_GameRect3");

			if (rect.z <= 0) {
				return(3);
			}

			rect = Shader.GetGlobalVector("_GameRect4");

			if (rect.z <= 0) {
				return(4);
			}

			Debug.Log("cant find");

			return(0);
		}*/
