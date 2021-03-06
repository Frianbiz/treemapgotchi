using Wrld.Common.Maths;
using Wrld.MapCamera;
using Wrld.Materials;
using Wrld.Meshes;
using Wrld.Space;
using Wrld.Concurrency;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Wrld
{
    public class NativePluginRunner
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        public const string DLL = "eegeo-stream-app";
#elif UNITY_IOS && !UNITY_EDITOR
        public const string DLL = "__Internal";
#else
        public const string DLL = "StreamAlpha";
#endif

        [DllImport(DLL)]
        private static extern void Initialize(
            int screenWidth,
            int screenHeight,
            float screenDPI,
            [MarshalAs(UnmanagedType.LPStr)]string apiKey,
            [MarshalAs(UnmanagedType.LPStr)]string assetPath,
            ref ConfigParams.NativeConfig config,
            MeshUploader.AllocateUnpackedMeshCallback allocateUnpackedMesh,
            MeshUploader.UploadUnpackedMeshCallback uploadUnpackedMesh,
            MapGameObjectScene.AddMeshCallback addMesh,
            MapGameObjectScene.DeleteMeshCallback deleteMesh,
            MapGameObjectScene.VisibilityCallback setVisible,
            CameraApi.CameraEventCallback cameraEventCallback,
            AssertHandler.HandleAssertCallback assertHandlerCallback,
            TextureLoadHandler.AllocateTextureBufferCallback allocateTextureBuffer,
            TextureLoadHandler.BeginUploadTextureBufferCallback beginUploadTextureBuffer,
            TextureLoadHandler.ReleaseTextureCallback releaseTexture,
            ThreadService.CreateThreadDelegate createThread,
            ThreadService.JoinThreadDelegate joinThread,
            [MarshalAs(UnmanagedType.LPStr)]string coverageTreeUrl,
            [MarshalAs(UnmanagedType.LPStr)]string themeUrl
            );

        [DllImport(DLL)]
        private static extern void Update(float t);

        [DllImport(DLL)]
        private static extern void Destroy();

        [DllImport(DLL)]
        private static extern IntPtr GetAppInterface();

        [DllImport(DLL)]
        private static extern void Pause();

        [DllImport(DLL)]
        private static extern void Resume();

        public static IntPtr API;

        MaterialRepository m_materialRepository;
        TextureLoadHandler m_textureLoadHandler;
        MapGameObjectScene m_mapGameObjectScene;
        StreamingUpdater m_streamingUpdater;

        private bool m_isRunning = false;

        private static string GetStreamingAssetsDir()
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN || UNITY_STANDALONE
            var path = Application.streamingAssetsPath + "/WrldResources";
#elif UNITY_IOS
            var path = "Data/Raw/WrldResources/";
#elif UNITY_ANDROID
            var path = "jar:file://" + Application.dataPath + "!/assets/";
#endif
            return path;
        }


        public NativePluginRunner(string apiKey, TextureLoadHandler textureLoadHandler, MaterialRepository materialRepository, MapGameObjectScene mapGameObjectScene, ConfigParams config)
        {
            // Initialize singleton - this is creepy and should be scheduled as debt
            // Deliberately not assigning it to keep warnings out of the Unity console
            new ThreadService();

            m_textureLoadHandler = textureLoadHandler;
            m_materialRepository = materialRepository;
            m_mapGameObjectScene = mapGameObjectScene;
            m_streamingUpdater = new StreamingUpdater();

            var nativeConfig = config.GetNativeConfig();
            var pathString = GetStreamingAssetsDir();

            Initialize(Screen.width, Screen.height, Screen.dpi,
                apiKey,
                pathString,
                ref nativeConfig,
                new MeshUploader.AllocateUnpackedMeshCallback(MeshUploader.AllocateUnpackedMesh),
                new MeshUploader.UploadUnpackedMeshCallback(MeshUploader.UploadUnpackedMesh),
                new MapGameObjectScene.AddMeshCallback(MapGameObjectScene.AddMesh), 
                new MapGameObjectScene.DeleteMeshCallback(MapGameObjectScene.DeleteMesh),
                new MapGameObjectScene.VisibilityCallback(MapGameObjectScene.SetVisible),
                new CameraApi.CameraEventCallback(CameraApi.OnCameraEvent),
                new AssertHandler.HandleAssertCallback(AssertHandler.HandleAssert),
                new TextureLoadHandler.AllocateTextureBufferCallback(TextureLoadHandler.AllocateTextureBuffer), 
                new TextureLoadHandler.BeginUploadTextureBufferCallback(TextureLoadHandler.BeginUploadTextureBuffer), 
                new TextureLoadHandler.ReleaseTextureCallback(TextureLoadHandler.ReleaseTexture),
                new ThreadService.CreateThreadDelegate(ThreadService.CreateThread),
                new ThreadService.JoinThreadDelegate(ThreadService.JoinThread),
                config.CoverageTreeManifestUrl,
                config.ThemeManifestUrl
                );

            API = GetAppInterface();
            Debug.Assert(API != IntPtr.Zero);

            m_isRunning = true;
        }

        public void StreamResourcesForCamera(UnityEngine.Camera zeroBasedCameraECEF, DoubleVector3 cameraOriginECEF, DoubleVector3 interestPointECEF)
        {
            m_streamingUpdater.Update(zeroBasedCameraECEF, cameraOriginECEF, interestPointECEF);
        }

        public void Update()
        {
            if (m_isRunning)
            {
                Update(Time.deltaTime);
            }

            m_textureLoadHandler.Update();
            m_materialRepository.Update();
        }

        public void UpdateTransforms(ITransformUpdateStrategy transformUpdateStrategy)
        {
            m_mapGameObjectScene.UpdateTransforms(transformUpdateStrategy);
        }

        public void UpdateCollisions(ConfigParams.CollisionConfig collisions)
        {
            m_mapGameObjectScene.ChangeCollision(collisions);
        }

        public void OnDestroy()
        {
            Destroy();
        }

        public void OnApplicationPaused()
        {
            if (m_isRunning)
            {
                Pause();
                m_isRunning = false;                
            }
        }

        public void OnApplicationResumed()
        {
            if (!m_isRunning)
            {
                Resume();
                m_isRunning = true;
            }
        }
    }
}

