using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Card;

namespace Pvz.Assets.Scr.HUD;

[Scene]
public partial class CardPool : TextureRect
{
    [Export(PropertyHint.Dir)]
    private string cardSceneDir;

    [Export(PropertyHint.File, "*.tscn")]
    private string excludeCardScene;

    [Node("GridContainer")]
    private GridContainer gridContainer;

    private List<string> cardScenePathList = new();

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        InitializePool();
    }

    public void InitializePool()
    {
        cardScenePathList = DirAccess.GetFilesAt(cardSceneDir)
            .Select(file => $"{cardSceneDir}/{file}")
            .Where(path => path.EndsWith(".tscn"))
            .Where(path => path != excludeCardScene)
            .Distinct()
            .ToList();

        foreach (BaseCard card in cardScenePathList
                     .Select(path => ResourceLoader.Load<PackedScene>(path)?.InstantiateOrNull<BaseCard>())
                     .Where(card => card != null)
                     .ToList())
        {
            gridContainer.AddChild(card);
        }
    }

    public IEnumerable<BaseCard> GetAll()
    {
        return gridContainer.GetChildren()
            .Where(node => node is BaseCard)
            .Cast<BaseCard>()
            .ToList();
    }
}