using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.ACC.CSS.Systems.AlchemyMagic;
using Server.ACC.CSS.Systems.FishingMagic;

public class CodexSaleGump : Gump
{
    private const int ItemsPerPage = 24;
    private const int ItemsPerRow = 6;
    private const int RowsPerPage = 4;

    private List<SpellbookInfo> _spellbooks;
    private int _page;

    public CodexSaleGump(int page = 0) : base(50, 50)
    {
        _page = page;
        _spellbooks = InitializeSpellbooks();

        Closable = true;
        Disposable = true;
        Dragable = true;
        Resizable = false;

        AddPage(0);
        AddBackground(155, 33, 798, 630, 9200);

        AddSpellbooks();
        AddPageButtons();
    }

    private List<SpellbookInfo> InitializeSpellbooks()
    {
        // Initialize your spellbooks here with their names and required virtue stones
        return new List<SpellbookInfo>
        {
            new SpellbookInfo("AlchemySpellbook", new[] { "CompassionStone", "HonestyStone", "CompassionStone" }),
            new SpellbookInfo("FishingSpellbook", new[] { "HonorStone", "JusticeStone", "HonestyStone" }),
			new SpellbookInfo("EvalIntSpellbook", new[] { "HumilityStone", "JusticeStone", "SacrificeStone" }),
			new SpellbookInfo("ArcherySpellbook", new[] { "ValorStone", "HonestyStone", "HonorStone" }),
			new SpellbookInfo("MagerySpellbook", new[] { "HonestyStone", "SpiritualityStone", "HonestyStone" }),
			new SpellbookInfo("ArmsLoreSpellbook", new[] { "SpiritualityStone", "HonorStone", "HumilityStone" }),
			new SpellbookInfo("AnimalTamingSpellbook", new[] { "SacrificeStone", "JusticeStone", "HonestyStone" }),
			new SpellbookInfo("AnimalLoreSpellbook", new[] { "HonorStone", "ValorStone", "ValorStone" }),
			new SpellbookInfo("CarpentrySpellbook", new[] { "HonestyStone", "SpiritualityStone", "CompassionStone" }),
			new SpellbookInfo("CartographySpellbook", new[] { "JusticeStone", "SpiritualityStone", "HonestyStone" }),
			new SpellbookInfo("TasteIDSpellbook", new[] { "SacrificeStone", "JusticeStone", "HonestyStone" }),
			new SpellbookInfo("CookingSpellbook", new[] { "HonorStone", "HonestyStone", "SacrificeStone" }),
			new SpellbookInfo("DiscordanceSpellbook", new[] { "HonorStone", "SpiritualityStone", "HonestyStone" }),
			new SpellbookInfo("FencingSpellbook", new[] { "HonorStone", "JusticeStone", "JusticeStone" }),
			new SpellbookInfo("FletchingSpellbook", new[] { "HonorStone", "JusticeStone", "SacrificeStone" }),
			new SpellbookInfo("ForensicsSpellbook", new[] { "ValorStone", "HonestyStone", "JusticeStone" }),
			new SpellbookInfo("WrestlingSpellbook", new[] { "HumilityStone", "HonorStone", "SpiritualityStone" }),
			new SpellbookInfo("ParrySpellbook", new[] { "SpiritualityStone", "HonestyStone", "HonestyStone" }),
			new SpellbookInfo("HealingSpellbook", new[] { "HonestyStone", "JusticeStone", "ValorStone" }),
			new SpellbookInfo("DetectHiddenSpellbook", new[] { "CompassionStone", "HumilityStone", "ValorStone" }),
			new SpellbookInfo("ProvocationSpellbook", new[] { "SacrificeStone", "SacrificeStone", "HonorStone" }),
			new SpellbookInfo("LockpickingSpellbook", new[] { "JusticeStone", "HonorStone", "SpiritualityStone" }),
			new SpellbookInfo("MacingSpellbook", new[] { "CompassionStone", "HumilityStone", "ValorStone" }),
			new SpellbookInfo("MeditationSpellbook", new[] { "SpiritualityStone", "JusticeStone", "HumilityStone" }),
			new SpellbookInfo("BeggingSpellbook", new[] { "CompassionStone", "HonorStone", "HonestyStone" }),
			new SpellbookInfo("MiningSpellbook", new[] { "HumilityStone", "HonorStone", "HonorStone" }),
			new SpellbookInfo("ChivalrySpellbook", new[] { "HonestyStone", "HumilityStone", "ValorStone" }),
			new SpellbookInfo("StealingSpellbook", new[] { "HonestyStone", "ValorStone", "HumilityStone" }),
			new SpellbookInfo("InscribeSpellbook", new[] { "HonorStone", "SacrificeStone", "SpiritualityStone" }),
			new SpellbookInfo("NinjitsuSpellbook", new[] { "SpiritualityStone", "ValorStone", "CompassionStone" }),
			new SpellbookInfo("HidingSpellbook", new[] { "CompassionStone", "HumilityStone", "JusticeStone" }),
			new SpellbookInfo("StealthSpellbook", new[] { "HonorStone", "SpiritualityStone", "SacrificeStone" }),
			new SpellbookInfo("BlacksmithSpellbook", new[] { "HumilityStone", "HumilityStone", "JusticeStone" }),
			new SpellbookInfo("TacticsSpellbook", new[] { "CompassionStone", "JusticeStone", "SpiritualityStone" }),
			new SpellbookInfo("SwordsSpellbook", new[] { "JusticeStone", "CompassionStone", "HonorStone" }),
			new SpellbookInfo("TailoringSpellbook", new[] { "HonestyStone", "SacrificeStone", "HumilityStone" }),
			new SpellbookInfo("NecromancySpellbook", new[] { "SacrificeStone", "SacrificeStone", "JusticeStone" }),
			new SpellbookInfo("TrackingSpellbook", new[] { "SacrificeStone", "JusticeStone", "SpiritualityStone" }),
			new SpellbookInfo("RemoveTrapSpellbook", new[] { "JusticeStone", "SacrificeStone", "HonestyStone" }),
			new SpellbookInfo("VeterinarySpellbook", new[] { "JusticeStone", "HumilityStone", "SacrificeStone" }),
			new SpellbookInfo("MusicianshipSpellbook", new[] { "CompassionStone", "CompassionStone", "HonorStone" }),
			new SpellbookInfo("CampingSpellbook", new[] { "SacrificeStone", "HonorStone", "CompassionStone" }),
			new SpellbookInfo("LumberjackingSpellbook", new[] { "ValorStone", "SpiritualityStone", "HonorStone" }),
            // Add the rest of your spellbooks here...
        };
    }

    private void AddSpellbooks()
    {
        int startIndex = _page * ItemsPerPage;
        int endIndex = Math.Min(startIndex + ItemsPerPage, _spellbooks.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            int relativeIndex = i - startIndex;
            int row = relativeIndex / ItemsPerRow;
            int col = relativeIndex % ItemsPerRow;

            int x = 211 + (col * 122);
            int y = 89 + (row * 148);

            AddSpellbookItem(x, y, i, _spellbooks[i]);
        }
    }

    private void AddSpellbookItem(int x, int y, int index, SpellbookInfo spellbook)
    {
        AddBackground(x - 40, y - 25, 120, 140, 83);
		AddItem(x, y + 30, 3834, 0);
        AddButton(x + 8, y + 60, 2472, 2473, index + 1, GumpButtonType.Reply, 0);

        AddImage(x - 35, y + 34, 6250, 0);
        AddImage(x - 35, y + 61, 6249, 0);

        AddHtml(x - 29, y - 15, 100, 20, string.Format("<basefont color=#FFFFFF><center>{0}</center></basefont>", spellbook.Name), false, false);

        for (int i = 0; i < spellbook.RequiredStones.Length; i++)
        {
            AddItem(x + (i * 20) -40, y + 10, GetVirtueStoneItemID(spellbook.RequiredStones[i]));
        }
    }

    private int GetVirtueStoneItemID(string stoneName)
    {
        switch (stoneName)
        {
            case "CompassionStone":
                return 0x1869;
            case "HonestyStone":
                return 0x186A;
            case "HonorStone":
                return 0x186B;
            case "JusticeStone":
                return 0x186C;
            case "SacrificeStone":
                return 0x186D;
            case "SpiritualityStone":
                return 0x186E;
            case "ValorStone":
                return 0x186F;
            case "HumilityStone":
                return 0x1870;
            default:
                return 0x1869; // Default to CompassionStone graphic if not found
        }
    }

    private void AddPageButtons()
    {
        if (_page > 0)
        {
            AddButton(911, 621, 250, 251, 1000, GumpButtonType.Reply, 0); // Previous page
        }

        if ((_page + 1) * ItemsPerPage < _spellbooks.Count)
        {
            AddButton(934, 621, 252, 253, 1001, GumpButtonType.Reply, 0); // Next page
        }
    }

    public override void OnResponse(NetState sender, RelayInfo info)
    {
        Mobile from = sender.Mobile;

        switch (info.ButtonID)
        {
            case 1000: // Previous page
                from.SendGump(new CodexSaleGump(_page - 1));
                break;
            case 1001: // Next page
                from.SendGump(new CodexSaleGump(_page + 1));
                break;
            default:
                if (info.ButtonID > 0 && info.ButtonID <= _spellbooks.Count)
                {
                    SpellbookInfo selectedBook = _spellbooks[info.ButtonID - 1];
                    if (HasRequiredStones(from, selectedBook.RequiredStones))
                    {
                        RemoveRequiredStones(from, selectedBook.RequiredStones);
                        GiveSpellbook(from, selectedBook.Name);
                        from.SendMessage(string.Format("You have successfully purchased the {0}.", selectedBook.Name));
                    }
                    else
                    {
                        from.SendMessage("You do not have the required virtue stones to purchase this spellbook.");
                    }
                }
                break;
        }
    }

    private bool HasRequiredStones(Mobile from, string[] requiredStones)
    {
        Container pack = from.Backpack;

        if (pack == null)
            return false;

        foreach (string stoneName in requiredStones)
        {
            if (!FindItemInPack(pack, stoneName))
            {
                return false;
            }
        }

        return true;
    }

    private bool FindItemInPack(Container pack, string stoneName)
    {
        foreach (Item item in pack.Items)
        {
            if (item is Container subContainer)
            {
                if (FindItemInPack(subContainer, stoneName))
                {
                    return true;
                }
            }
            else if (item.Name == stoneName)
            {
                return true;
            }
        }
        return false;
    }

    private void RemoveRequiredStones(Mobile from, string[] requiredStones)
    {
        Container pack = from.Backpack;

        if (pack == null)
            return;

        foreach (string stoneName in requiredStones)
        {
            RemoveItemFromPack(pack, stoneName);
        }
    }

    private void RemoveItemFromPack(Container pack, string stoneName)
    {
        foreach (Item item in pack.Items)
        {
            if (item is Container subContainer)
            {
                RemoveItemFromPack(subContainer, stoneName);
            }
            else if (item.Name == stoneName)
            {
                item.Delete();
                return;
            }
        }
    }

    private void GiveSpellbook(Mobile from, string spellbookName)
    {
        Item spellbook = null;

        switch (spellbookName)
        {
            case "AlchemySpellbook":
                spellbook = new AlchemySpellbook();
                break;
            case "FishingSpellbook":
                spellbook = new FishingSpellbook();
                break;
            // Add cases for other spellbooks...
        }

        if (spellbook != null)
        {
            from.AddToBackpack(spellbook);
        }
    }
}

public class SpellbookInfo
{
    public string Name { get; private set; }
    public string[] RequiredStones { get; private set; }

    public SpellbookInfo(string name, string[] requiredStones)
    {
        Name = name;
        RequiredStones = requiredStones;
    }
}
