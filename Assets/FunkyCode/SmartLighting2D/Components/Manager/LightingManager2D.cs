using UnityEngine;
using FunkyCode.LightingSettings;

#if UNITY_EDITOR
	using UnityEditor;
#endif

namespace FunkyCode
{
	[ExecuteInEditMode] 
	public class LightingManager2D : LightingMonoBehaviour
	{
		private static LightingManager2D instance;

		[SerializeField]
		public LightingCameras cameras = new LightingCameras();

		public int version = 0;
		public string version_string = "";

		public LightingSettings.Profile setProfile;
		public LightingSettings.Profile profile;

		// editor foldouts (avoid reseting after compiling script)
		public bool[] foldout_cameras = new bool[10];

		public bool[,] foldout_lightmapPresets = new bool[10, 10];
		public bool[,] foldout_lightmapMaterials = new bool[10, 10];

		// Sets Lighting Main Profile Settings for Lighting2D at the start of the scene
		private static bool initialized = false; 

		public Camera GetCamera(int id)
		{
			if (cameras.Length <= id)
			{
				return(null);
			}

			return(cameras.Get(id).GetCamera());
		}

		public static void ForceUpdate() {}
		
		static public LightingManager2D Get()
		{
			if (instance != null)
			{
				return(instance);
			}

			foreach(LightingManager2D manager in UnityEngine.Object.FindObjectsOfType(typeof(LightingManager2D)))
			{
				instance = manager;

				return(instance);
			}

			// create new light manager
			GameObject gameObject = new GameObject("Lighting Manager 2D");

			instance = gameObject.AddComponent<LightingManager2D>();

			instance.transform.position = Vector3.zero;

			instance.version = Lighting2D.VERSION;

			instance.version_string = Lighting2D.VERSION_STRING;

			return(instance);
		}

		public void Awake()
		{
			if (cameras == null)
			{
				cameras = new LightingCameras();
			}
			
			if (instance != null && instance != this)
			{
				switch(Lighting2D.ProjectSettings.managerInstance)
				{
					case LightingSettings.ManagerInstance.Static:
					case LightingSettings.ManagerInstance.DontDestroyOnLoad:
						
						Debug.LogWarning("Smart Lighting2D: Lighting Manager duplicate was found, new instance destroyed.", gameObject);

						foreach(LightingManager2D manager in UnityEngine.Object.FindObjectsOfType(typeof(LightingManager2D)))
						{
							if (manager != instance)
							{
								manager.DestroySelf();
							}
						}

						return; // Cancel Initialization

					case LightingSettings.ManagerInstance.Dynamic:

						instance = this;
						
						Debug.LogWarning("Smart Lighting2D: Lighting Manager duplicate was found, old instance destroyed.", gameObject);

						foreach(LightingManager2D manager in UnityEngine.Object.FindObjectsOfType(typeof(LightingManager2D)))
						{
							if (manager != instance)
							{
								manager.DestroySelf();
							}
						}

					break;
				}
			}

			LightingManager2D.initialized = false;

			SetupProfile();

			if (Application.isPlaying)
			{
				if (Lighting2D.ProjectSettings.managerInstance == LightingSettings.ManagerInstance.DontDestroyOnLoad)
				{
					DontDestroyOnLoad(instance.gameObject);
				}
			}
		}

		private void Update()
		{
			if (Lighting2D.disable)
			{
				return;
			}

			ForceUpdate(); // for late update method?

			if (profile != null)
			{
				if (Lighting2D.Profile != profile)
				{
					Lighting2D.UpdateByProfile(profile);
				}
			}
		}

		private void LateUpdate()
		{
			if (Lighting2D.disable)
			{
				return;
			}

			UpdateInternal();
			
			if (Lighting2D.Profile.qualitySettings.updateMethod == LightingSettings.UpdateMethod.LateUpdate)
			{
				Rendering.Manager.Main.Render();
			}
		}

		public void SetupProfile()
		{
			if (LightingManager2D.initialized)
			{
				return;
			}

			LightingManager2D.initialized = true;

			LightingSettings.Profile profile = Lighting2D.Profile;
			
			Lighting2D.UpdateByProfile(profile);

			Lighting2D.materials.Reset();
		}

		public void UpdateInternal()
		{
			if (Lighting2D.disable)
			{
				return;
			}

			for(int id = 0; id < CameraTransform.List.Count; id++)
			{
				CameraTransform.List[id].Update();
			}
	
			SetupProfile();

			Rendering.Manager.Main.InternalUpdate();
		}

		public bool IsSceneView() // overlay
		{
			for(int i = 0; i < cameras.Length; i++)
			{
				CameraSettings cameraSetting = cameras.Get(i);

				if (cameraSetting.cameraType == CameraSettings.CameraType.SceneView)
				{
					for(int b = 0; b < cameraSetting.Lightmaps.Length; b++)
					{
						CameraLightmap bufferPreset = cameraSetting.GetLightmap(b);
				
						if (bufferPreset.overlay == CameraLightmap.Overlay.Enabled)
						{
							return(true);
						}
					}
				}
			}
			
			return(false);
		}

		private void OnDisable()
		{
			if (profile != null)
			{
				if (Application.isPlaying)
				{
					if (setProfile != profile)
					{
						if (Lighting2D.Profile == profile)
						{
							Lighting2D.RemoveProfile();
						}
					}
				}
			}

			#if UNITY_EDITOR

				#if UNITY_2019_1_OR_NEWER

					SceneView.beforeSceneGui -= OnSceneView;
					//SceneView.duringSceneGui -= OnSceneView;

				#else

					SceneView.onSceneGUIDelegate -= OnSceneView;

				#endif

			#endif
		}

		public void UpdateProfile()
		{
			if (setProfile == null)
			{
				setProfile = Lighting2D.ProjectSettings.Profile;
			} 

			if (Application.isPlaying)
			{
				profile = UnityEngine.Object.Instantiate(setProfile);
			}
				else
			{
				profile = setProfile;
			}
		}

		private void OnEnable()
		{
			foreach(OnRenderMode onRenderMode in UnityEngine.Object.FindObjectsOfType(typeof(OnRenderMode)))
			{
				onRenderMode.DestroySelf();
			}

			Scriptable.LightSprite2D.List.Clear();

			UpdateProfile();
			Rendering.Manager.Main.UpdateMaterials();

			for(int i = 0; i < cameras.Length; i++)
			{
				CameraSettings cameraSetting = cameras.Get(i);

				for(int b = 0; b < cameraSetting.Lightmaps.Length; b++)
				{
					CameraLightmap bufferPreset = cameraSetting.GetLightmap(b);
			
					foreach(Material material in bufferPreset.GetMaterials().materials)
					{
						if (material == null)
						{
							continue;
						}

						Camera camera = cameraSetting.GetCamera();

						if (cameraSetting.cameraType == CameraSettings.CameraType.SceneView)
						{
							LightmapMaterials.ClearMaterial(material);
						}
					}
				}
			}
		
			Update();
			LateUpdate();
		
			#if UNITY_EDITOR
				#if UNITY_2019_1_OR_NEWER
					SceneView.beforeSceneGui += OnSceneView;
					//SceneView.duringSceneGui += OnSceneView;
				#else
					SceneView.onSceneGUIDelegate += OnSceneView;
				#endif	
			#endif	
		}

		private void OnRenderObject()
		{
			if (Lighting2D.RenderingMode != RenderingMode.OnPostRender)
			{
				return;
			}
			
			foreach(LightMainBuffer2D buffer in LightMainBuffer2D.List)
			{
				Rendering.LightMainBuffer.DrawPost(buffer);
			}
		}

		private void OnDrawGizmos()
		{
			if (Lighting2D.ProjectSettings.editorView.drawGizmos != EditorDrawGizmos.Always)
			{
				return;
			}

			DrawGizmos();
		}
		
		private void DrawGizmos()
		{
			if (!isActiveAndEnabled)
			{
				return;
			}

			Gizmos.color = new Color(0, 1f, 1f);

			if (Lighting2D.ProjectSettings.editorView.drawGizmosBounds == EditorGizmosBounds.Enabled)
			{
				for(int i = 0; i < cameras.Length; i++)
				{
					CameraSettings cameraSetting = cameras.Get(i);

					Camera camera = cameraSetting.GetCamera();

					if (camera != null)
					{
						Rect cameraRect = CameraTransform.GetWorldRect(camera);

						GizmosHelper.DrawRect(transform.position, cameraRect);
					}
				}
			}

			for(int i = 0; i < Scriptable.LightSprite2D.List.Count; i++)
			{
				Scriptable.LightSprite2D light = Scriptable.LightSprite2D.List[i];

				Rect rect = light.lightSpriteShape.GetWorldRect();

				Gizmos.color = new Color(1f, 0.5f, 0.25f);

				GizmosHelper.DrawPolygon(light.lightSpriteShape.GetSpriteWorldPolygon(), transform.position);

				Gizmos.color = new Color(0, 1f, 1f);
				GizmosHelper.DrawRect(transform.position, rect);
			}
		}

		#if UNITY_EDITOR
			static private void OnSceneView(SceneView sceneView)
			{
				LightingManager2D manager = LightingManager2D.Get();
		
				if (!manager.IsSceneView())
				{
					return;
				}

				if (Application.isPlaying)
				{
					return;
				}

				ForceUpdate();

				Rendering.Manager.Main.Render();
			}
		#endif
	}
}