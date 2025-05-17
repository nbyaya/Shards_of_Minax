using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SixFoldPurgeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Six-Fold Purge"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Vesper Hexaria*, Runic Architect of Castle British.\n\n" +
                    "She hovers near a levitating blueprint etched with glowing lines, her robes shimmering with arcane patterns that shift as she speaks.\n\n" +
                    "“My seals were once sanctified by six guardians of the stars… until *it* came.”\n\n" +
                    "“The Seraph corrupted them—unraveled the purity of the runes and bound its wings in my wards. **The HexaSeraph now roosts in the Vault’s consecration halls**, and the air tastes of burned starlight.”\n\n" +
                    "“I have rewritten the glyphs. They will now bleed its wings. But the ritual is unfinished without a blade.”\n\n" +
                    "**Descend into Preservation Vault 44**. Slay the HexaSeraph. Let it fall in six-fold ruin.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we delay the unraveling… but not its hunger.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Seraph still sings. And every note fractures the Vault’s harmony.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The Vault quiets. My seals hold… for now.\n\n" +
                       "This is *GeminiFang*—once a ceremonial blade, now forged to sever corrupted lineage. Let it serve you, as you have served the balance.";
            }
        }

        public SixFoldPurgeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(HexaSeraph), "HexaSeraph", 1));
            AddReward(new BaseReward(typeof(GeminiFang), 1, "GeminiFang"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You’ve completed 'Six-Fold Purge'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VesperHexaria : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SixFoldPurgeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArchitect());
        }

        [Constructable]
        public VesperHexaria()
            : base("the Runic Architect", "Vesper Hexaria")
        {
        }

        public VesperHexaria(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 100, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale mystic tone
            HairItemID = 0x203B; // Long Hair
            HairHue = 1154; // Starfire silver
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1161, Name = "Starsealed Mantle" }); // Deep lunar blue
            AddItem(new Cloak() { Hue = 1153, Name = "Echoveil Drape" }); // Pale arcane shimmer
            AddItem(new ElvenBoots() { Hue = 2418, Name = "Glyphwalk Treads" }); // Ethereal green
            AddItem(new BodySash() { Hue = 1165, Name = "Hexbinding Sash" }); // Soft amethyst

            AddItem(new MagicWand() { Hue = 1175, Name = "Runic Alignstaff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1171;
            backpack.Name = "Vault-Glyph Satchel";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
