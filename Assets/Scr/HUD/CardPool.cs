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

    [Node("Rows")]
    private VBoxContainer rows;

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

        foreach (List<BaseCard> list in cardScenePathList
                     .Select(path => ResourceLoader.Load<PackedScene>(path)?.InstantiateOrNull<BaseCard>())
                     .Where(card => card != null)
                     .Select((n, index) => new { n, index })
                     .GroupBy(x => x.index / 8)
                     .Select(g => g.Select(x => x.n).ToList())
                     .ToList())
        {
            HBoxContainer row = new();
            rows.AddChild(row);
            foreach (BaseCard card in list)
            {
                row.AddChild(card);
            }
        }
    }

    public IEnumerable<BaseCard> GetAll()
    {
        return rows.GetChildren()
            .Where(node => node is HBoxContainer)
            .SelectMany(box => box.GetChildren().Where(node => node is BaseCard).Cast<BaseCard>().ToList())
            .ToList();
    }
}