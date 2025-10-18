using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerShootingSystem : SystemBase
{
    private InputHandler _inputHandler;
    
    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
        _inputHandler = GameObject.FindObjectOfType<InputHandler>();
        if(!_inputHandler)
            Debug.LogError("Failed to find inputHandler");
    }
    
    protected override void OnUpdate()
    {
        // if (!Input.GetKeyDown(KeyCode.Space))
        if (_inputHandler.reload /* temp stunning stuff */)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentEnabled<Stunned>(playerEntity, true);
        }
        if (!_inputHandler.reload /* temp stunning stuff */)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentEnabled<Stunned>(playerEntity, false);
        }
        if(!_inputHandler.jump)
        {
            return;
        }
        
        SpawnCubesConfig spawnCubesConfig = SystemAPI.GetSingleton<SpawnCubesConfig>();
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(WorldUpdateAllocator);
        foreach(RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>().WithDisabled<Stunned>())
        {
            Entity spawnedEntity = entityCommandBuffer.Instantiate(spawnCubesConfig.cubePrefabEntity);
            entityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
            {
                Position = localTransform.ValueRO.Position,
                Rotation = quaternion.identity,
                Scale = 1f
            });
        }
        entityCommandBuffer.Playback(EntityManager);
    }
}
