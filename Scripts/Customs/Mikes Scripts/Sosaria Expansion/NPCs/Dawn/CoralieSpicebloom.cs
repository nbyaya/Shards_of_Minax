using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SpicesInTheSwampQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Spices in the Swamp"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Coralie Spicebloom*, Dawn’s beloved herbal cook, tending to a pot simmering with wild roots and a bitter smile.\n\n" +
                    "She brushes strands of hair from her freckled face, sleeves rolled high, eyes reflecting both warmth and sorrow.\n\n" +
                    "\"My scouts—friends—lost to that beast. We went to the marsh to gather *swamp pepper*, the key to my rarest blends... but only I returned.\"\n\n" +
                    "\"The Doomscale Alligator lurks there, scales weeping acid that burns the earth itself. I can't fight, but you—you look like someone who can.\"\n\n" +
                    "**Slay the Doomscale Alligator** and bring me peace, and perhaps, one last taste of what we lost.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the spices rot, and memories with them. The marsh claims all who dare.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The alligator still breathes? Each day its presence poisons the land and the hearts of those left behind.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? It’s over?\n\n" +
                       "The marsh can breathe again… and so can I.\n\n" +
                       "*SkyleafTreads*—crafted with the lightest herbs and hope. Walk gently, and remember us.";
            }
        }

        public SpicesInTheSwampQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomscaleAlligator), "Doomscale Alligator", 1));
            AddReward(new BaseReward(typeof(SkyleafTreads), 1, "SkyleafTreads"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Spices in the Swamp'!");
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

    public class CoralieSpicebloom : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SpicesInTheSwampQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCook());
        }

        [Constructable]
        public CoralieSpicebloom()
            : base("the Herbal Cook", "Coralie Spicebloom")
        {
        }

        public CoralieSpicebloom(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 55);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1055; // Warm tan
            HairItemID = 0x203C; // Wavy Hair
            HairHue = 2124; // Auburn-red
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2117, Name = "Spice-Touched Dress" }); // Rich orange-red
            AddItem(new HalfApron() { Hue = 1358, Name = "Marshbloom Apron" }); // Earthy green
            AddItem(new Sandals() { Hue = 1272, Name = "Softstep Sandals" }); // Soft moss hue
            AddItem(new FlowerGarland() { Hue = 1153, Name = "Herbal Wreath" }); // Pale lavender flowers

            AddItem(new GourmandsFork() { Hue = 0, Name = "Spicebloom Fork" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Forager's Pouch";
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
