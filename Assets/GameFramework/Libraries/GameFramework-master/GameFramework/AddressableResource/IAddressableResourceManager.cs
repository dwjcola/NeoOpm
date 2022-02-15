using GameFramework.Download;
using GameFramework.FileSystem;
using GameFramework.ObjectPool;
using System;
using GameFramework.Resource;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.U2D;

namespace GameFramework.AddressableResource
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IAddressableResourceManager
    {
        
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset<TObject>(string assetName, LoadAssetCallbacks loadAssetCallbacks);
        /// <summary>
        /// 同步方式加载资源
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="loadAssetCallbacks"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        TObject LoadAssetSync<TObject>(string assetName);
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset<TObject>(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData);

        Task<TObject> GetJsonTable<TObject>(string jsonName);
        Task<Sprite> GetSprite(string atlas, string spname);
        Task<Texture2D> GetTexture(string textureName);
        Task<Sprite[]> GetSprites(string atlas);
        Task<Sprite> GetAtlasSprite(string atlas, string spname);
        Task<SpriteAtlas> GetAtlas(string atlas);
        Task<Material> GetMaterial(string path);
        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="assetKey"></param>
        /// <returns></returns>
        GameObject InstantiateAsset(object assetKey);
        AsyncOperationHandle<GameObject> InstantiateAssetAsync(object assetKey);

        /// <summary>
        /// 这个异步接口使用缓存实例化
        /// </summary>
        /// <param name="assetKey"></param>
        /// <param name="parameters"></param>
        /// <param name="insAction"></param>
        void InstantiateUseCache(object assetKey, InstantiationParameters parameters=default, Action<GameObject> insAction = null);
        
        /// <summary>
        /// 删除实例化
        /// </summary>
        /// <param name="asset"></param>
        void ReleaseInstantiateAsset(GameObject asset);

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData);

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        void UnloadScene(string sceneAssetName, SceneInstance sceneAsset, UnloadSceneCallbacks unloadSceneCallbacks);

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void UnloadScene(string sceneAssetName, SceneInstance sceneAsset, UnloadSceneCallbacks unloadSceneCallbacks, object userData);

        AsyncOperationHandle<SceneInstance> LoadSceneAsync(string sceneName, LoadSceneMode mode);
        AsyncOperationHandle<T> LoadAssetAsync<T>(string assetName,bool autoRelease=false);
        void LoadAssetAsync<TObject>(string assetName, Action<TObject> callback);
        Task<T> LoadAssetAsync22<T>(string assetName);
        Task<IList<T>> LoadAssetsAsync<T>(string lable, Action<T> action=null, bool autoRelease = false);
        void Release<TObject>(AsyncOperationHandle<TObject> handle);
    }
    
}