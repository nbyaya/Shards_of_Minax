using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    /// <summary>
    /// Glyph of Bounty ― “Imbues the map with a random commodity‐loot modifier.”  
    /// </summary>
    public class GlyphOfBounty : Item
    {
        private static readonly string[] _ModifierIDs = new[]
        {
            "IronIngot","DullCopperIngot","ShadowIronIngot","CopperIngot","BronzeIngot","GoldIngot","AgapiteIngot","VeriteIngot","ValoriteIngot",
            "Bacon","Ham","Sausage","RawChickenLeg","RawBird","RawLambLeg","RawRibs",
            "Board","BreadLoaf","ApplePie",
            "StarSapphire","Emerald","Sapphire","Ruby","Citrine","Amethyst","Tourmaline","Amber","Diamond",
            "BoltOfCloth","Cotton","Wool","Flax",
            "OakLog","AshLog","YewLog",
            "RandomMagicWeapon","RandomMagicArmor","RandomMagicClothing","RandomMagicJewelry",
            "MaxxiaScroll","SkillOrb","StatCapOrb",
            "DecorativeLootbox","GearLootbox","KingLootbox","MagicalLootbox","WorldLootbox",
            "StartingJewelryBox","StartingClothes","StartingMedicine","PersonalArmoire","StartingCrate",
            "TownCommodityBarrel","TownTreasureChest","AbandonedRefuseChest","BossTreasureBox",
            "MurderTreasureChest","RefuseOfTheFallen","TradeRouteChest",
            "StartingGarbage","StartingKitchen","StartingTreasureChest","ElderLootbox"
        };

        private const int DefaultItemID = 0x1F16; // pick your graphic
        private const int DefaultHue    = 0x47E;  // a rich hue

        [Constructable]
        public GlyphOfBounty() : base(DefaultItemID)
        {
            Name   = "Glyph of Bounty";
            Hue    = DefaultHue;
            Weight = 1.0;
        }

        public GlyphOfBounty(Serial serial) : base(serial) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Imbues a magic map with a random commodity‐loot modifier");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // must be in backpack
                return;
            }

            from.SendMessage("Select the magic map to empower with Bounty.");
            from.Target = new BountyTarget(this);
        }

        private class BountyTarget : Target
        {
            private readonly GlyphOfBounty _glyph;

            public BountyTarget(GlyphOfBounty glyph) : base(12, false, TargetFlags.None)
            {
                _glyph = glyph;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_glyph.Deleted)
                    return;

                if (targeted is MagicMapBase map && map.IsChildOf(from.Backpack))
                {
                    // pick a random modifier ID
                    var id = _ModifierIDs[Utility.Random(_ModifierIDs.Length)];
                    map.AddModifier(id);

                    from.SendMessage($"A surge of insight flows through the parchment—“{id}” has been added!");
                    _glyph.Delete();
                }
                else
                {
                    from.SendMessage("That is not a magic map in your pack.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_glyph.Deleted)
                    from.SendMessage("You decide against inscribing the glyph.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
