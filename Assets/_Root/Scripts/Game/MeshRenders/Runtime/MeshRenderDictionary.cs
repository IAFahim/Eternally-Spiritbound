using System.Collections.Generic;
using Pancake.Common;
using Unity.Collections;
using UnityEngine;

namespace _Root.Scripts.Game.MeshRenders.Runtime
{
    public static class MeshRenderDictionary
    {
        private static readonly Dictionary<Mesh, RenderGroupInfo> dictionary = new();
        private static bool isInitialized;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            Cleanup();
            App.AddListener(EUpdateMode.Update, Render);
        }

        private static void Cleanup()
        {
            App.RemoveListener(EUpdateMode.Update, Render);
            foreach (var renderGroup in dictionary.Values)
            {
                renderGroup.Dispose();
            }

            dictionary.Clear();
            isInitialized = false;
        }

        public static int AddToRender(Mesh mesh, Material material, int subMeshIndex, Vector3[] points,
            Vector3 size)
        {
            if (dictionary.TryGetValue(mesh, out var renderGroup))
            {
                renderGroup.renderParams.material = material;
                return renderGroup.Add(points, size);
            }

            var renderParams = new RenderParams(material);
            renderGroup = new RenderGroupInfo(renderParams, points, size);
            dictionary.Add(mesh, renderGroup);

            return 0;
        }

        public static void Render()
        {
            foreach (var (mesh, renderGroup) in dictionary)
            {
                if (renderGroup.nativeMatrices.Length > 0)
                {
                    Graphics.RenderMeshInstanced(renderGroup.renderParams, mesh, 0, renderGroup.nativeMatrices);
                }
            }
        }

        public static void RemoveFromRender(Mesh mesh, int instanceId)
        {
            if (!dictionary.TryGetValue(mesh, out var renderGroup))
            {
                return;
            }

            if (instanceId < 0 || instanceId >= renderGroup.instanceIdStartEndInNativeArray.Count)
            {
                throw new System.ArgumentOutOfRangeException($"Invalid instance ID: {instanceId}");
            }

            renderGroup.Remove(instanceId);

            if (renderGroup.nativeMatrices.Length == 0)
            {
                renderGroup.Dispose();
                dictionary.Remove(mesh);

                if (dictionary.Count == 0)
                {
                    App.RemoveListener(EUpdateMode.Update, Render);
                    isInitialized = false;
                }
            }
        }

        public static void Move(Mesh mesh, int instanceId, Vector3[] positions)
        {
            if (dictionary.TryGetValue(mesh, out var renderGroup))
            {
                renderGroup.MoveSlice(instanceId, positions);
            }
            else throw new System.ArgumentException($"Mesh {mesh.name} not found in dictionary");
        }

        public static void Move(Mesh mesh, int instanceId, int startOffset, Vector3 position)
        {
            if (dictionary.TryGetValue(mesh, out var renderGroup))
            {
                renderGroup.MoveSingle(instanceId, startOffset, position);
            }
            else throw new System.ArgumentException($"Mesh {mesh.name} not found in dictionary");
        }
    }

    public class RenderGroupInfo
    {
        public RenderParams renderParams;
        public List<(int startIndex, int endIndex)> instanceIdStartEndInNativeArray;
        public NativeArray<Matrix4x4> nativeMatrices;

        public RenderGroupInfo(RenderParams renderParams, Vector3[] points, Vector3 size)
        {
            this.renderParams = renderParams;
            instanceIdStartEndInNativeArray = new(points.Length);
            nativeMatrices = new NativeArray<Matrix4x4>(points.Length, Allocator.Persistent);
            for (var i = 0; i < points.Length; i++)
            {
                nativeMatrices[i] = Matrix4x4.TRS(points[i], Quaternion.identity, size);
            }

            instanceIdStartEndInNativeArray.Add((0, points.Length));
        }

        public int Add(Vector3[] points, Vector3 size)
        {
            var instanceId = instanceIdStartEndInNativeArray.Count;
            var newNativeMatrices =
                new NativeArray<Matrix4x4>(nativeMatrices.Length + points.Length, Allocator.Persistent);
            for (var i = 0; i < nativeMatrices.Length; i++)
            {
                newNativeMatrices[i] = nativeMatrices[i];
            }

            for (var i = 0; i < points.Length; i++)
            {
                newNativeMatrices[i + nativeMatrices.Length] = Matrix4x4.TRS(points[i], Quaternion.identity, size);
            }

            nativeMatrices.Dispose();
            nativeMatrices = newNativeMatrices;
            instanceIdStartEndInNativeArray.Add((nativeMatrices.Length - points.Length, nativeMatrices.Length));
            return instanceId;
        }

        public void Remove(int instanceId)
        {
            var (startIndex, endIndex) = instanceIdStartEndInNativeArray[instanceId];
            var newNativeMatrices =
                new NativeArray<Matrix4x4>(nativeMatrices.Length - (endIndex - startIndex), Allocator.Persistent);
            for (var i = 0; i < startIndex; i++)
            {
                newNativeMatrices[i] = nativeMatrices[i];
            }

            for (var i = endIndex; i < nativeMatrices.Length; i++)
            {
                newNativeMatrices[i - (endIndex - startIndex)] = nativeMatrices[i];
            }

            nativeMatrices.Dispose();
            nativeMatrices = newNativeMatrices;
            instanceIdStartEndInNativeArray.RemoveAt(instanceId);
        }

        public void MoveSingle(int instanceId, int startOffset, Vector3 position)
        {
            if (instanceId < 0 || instanceId >= instanceIdStartEndInNativeArray.Count)
            {
                throw new System.ArgumentOutOfRangeException($"Invalid instance ID: {instanceId}");
            }

            var (startIndex, endIndex) = instanceIdStartEndInNativeArray[instanceId];
            if (startOffset < 0 || startOffset >= endIndex - startIndex)
            {
                throw new System.ArgumentOutOfRangeException($"Invalid start offset: {startOffset}");
            }

            var matrix = nativeMatrices[startIndex + startOffset];
            matrix.SetColumn(3, position);
            nativeMatrices[startIndex + startOffset] = matrix;
        }

        public void MoveSlice(int instanceId, Vector3[] positions)
        {
            if (instanceId < 0 || instanceId >= instanceIdStartEndInNativeArray.Count)
            {
                throw new System.ArgumentOutOfRangeException($"Invalid instance ID: {instanceId}");
            }

            var (startIndex, endIndex) = instanceIdStartEndInNativeArray[instanceId];

            for (var i = startIndex; i < endIndex; i++)
            {
                var matrix = nativeMatrices[i];
                matrix.SetColumn(3, positions[i - startIndex]);
                nativeMatrices[i] = matrix;
            }
        }


        public void Dispose()
        {
            nativeMatrices.Dispose();
        }
    }
}