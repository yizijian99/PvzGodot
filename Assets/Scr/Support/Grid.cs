using System;
using Godot;
using GodotUtilities;

namespace Pvz.Assets.Scr.Support;

[Scene]
public partial class Grid : Control
{
    [Export]
    public RemoteTransform2D RemoteTransform2D { get; private set; }

    public Node2D Entity { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        UpdateEntity();
    }

    public bool Place(string scenePath, Node parent = null)
    {
        if (string.IsNullOrEmpty(scenePath)
            || Entity != null)
        {
            return false;
        }

        Node2D entity = ResourceLoader.Load<PackedScene>(scenePath)?.InstantiateOrNull<Node2D>();
        if (entity == null)
        {
            return false;
        }

        if (parent == null)
        {
            parent = this;
        }

        parent.AddChild(entity);

        RemoteTransform2D.RemotePath = RemoteTransform2D.GetPathTo(entity);
        Entity = entity;
        return true;
    }

    public void UpdateEntity()
    {
        try
        {
            Entity?.IsQueuedForDeletion();
        }
        catch (ObjectDisposedException e)
        {
            Entity = null;
        }
    }
}