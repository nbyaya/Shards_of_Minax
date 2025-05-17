using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HarvestOfHorrorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Harvest of Horror"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Tessa Ironroot*, the renowned armorer of Fawn, her forge crackling in the twilight.\n\n" +
                    "She wipes sweat from her brow, her hands blackened with soot but trembling.\n\n" +
                    "“Something’s wrong with the fields. My grain stores... they sprout these *foul vines* every dawn, twisted and black. The farmers whisper of a beast—**the Marrowth**—a creature feeding off our land’s lifeblood.”\n\n" +
                    "“If it isn’t stopped, we lose the harvest... and the town starves.”\n\n" +
                    "She gestures to a brazier at her forge, where embers glow faintly violet.\n\n" +
                    "“These blooms... they need fire. I’ll keep them at bay for now, but you must slay the Marrowth.”\n\n" +
                    "**Hunt down the Marrowth** in the Wilderness and bring peace to Fawn’s fields.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the harvest wither, and may we all learn to feast on shadows.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still prowls, then? The vines grow thicker. Soon, no flame will hold them back.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? The Marrowth falls? Praise the forge and flame!\n\n" +
                       "The fields might heal, in time... and so will we. Take this—**BonFire**. It’s more than warmth. It’s hope, born from battle.";
            }
        }

        public HarvestOfHorrorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Marrowth), "Marrowth", 1));
            AddReward(new BaseReward(typeof(BonFire), 1, "BonFire"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Harvest of Horror'!");
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

    public class TessaIronroot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HarvestOfHorrorQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public TessaIronroot()
            : base("the Armorer", "Tessa Ironroot")
        {
        }

        public TessaIronroot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Braided hair
            HairHue = 1148; // Fiery red
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedBustierArms() { Hue = 2425, Name = "Ironroot Forge Guard" }); // Deep ember red
            AddItem(new StuddedLegs() { Hue = 2401, Name = "Coal-Stained Greaves" });
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Sooted Handwraps" });
            AddItem(new HalfApron() { Hue = 1819, Name = "Forgemaster's Apron" });
            AddItem(new Boots() { Hue = 1812, Name = "Cinderwalk Boots" });

            AddItem(new SmithSmasher() { Hue = 2501, Name = "Tessa’s Tempering Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Forge Satchel";
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
