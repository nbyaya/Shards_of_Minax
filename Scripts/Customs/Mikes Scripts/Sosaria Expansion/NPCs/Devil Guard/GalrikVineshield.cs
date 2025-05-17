using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FleshAndSteelQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Flesh and Steel"; } }

        public override object Description
        {
            get
            {
                return
                    "Galrik Vineshield, the mountain herbalist, smells faintly of pine and crushed herbs.\n\n" +
                    "His eyes dart between you and a bundle of withered leaves.\n\n" +
                    "\"The land here... it’s bleeding magic. And that *thing* in the mines, the MinersFleshGolem... it’s sucking the life from my cures.\"\n\n" +
                    "\"It wasn’t always there. I think... I think some old druidic curse stirred when they dug too deep. The golem is part man, part steel—and it *feeds* on plant-life.\"\n\n" +
                    "\"If I’m to heal anyone, I need its sinew. Animated, yes—but steeped in energy I might turn back into something good.\"\n\n" +
                    "**Slay the MinersFleshGolem** and bring me what remains of its sinew. Or we all suffer as the land sickens.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we shall all wilt in the shadow of that thing. I pray you change your mind before more fall ill.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The golem still feeds... I feel it, drawing the life from every root and leaf. We won’t last long at this rate.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The air feels lighter already... and this sinew, it pulses with stolen life.\n\n" +
                       "Let me see if I can coax it into something useful again.\n\n" +
                       "**Take this, Oathcarver.** Carved for those who guard more than stone or gold—but life itself.";
            }
        }

        public FleshAndSteelQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MinersFleshGolem), "MinersFleshGolem", 1));
            AddReward(new BaseReward(typeof(OathcarverOfTheSilentGuard), 1, "Oathcarver"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Flesh and Steel'!");
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

    public class GalrikVineshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FleshAndSteelQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaponSmith());
        }

        [Constructable]
        public GalrikVineshield()
            : base("the Mountain Herbalist", "Galrik Vineshield")
        {
        }

        public GalrikVineshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1822; // Deep forest green hue
            FacialHairItemID = 0x2041; // Long beard
            FacialHairHue = 1822;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1422, Name = "Verdant Robe of Renewal" }); // Moss green
            AddItem(new LeatherGloves() { Hue = 1825, Name = "Briarwoven Gloves" });
            AddItem(new Sandals() { Hue = 1819, Name = "Earthenstride Sandals" });
            AddItem(new BodySash() { Hue = 1271, Name = "Sash of Druidic Binding" });
            AddItem(new WizardsHat() { Hue = 1420, Name = "Herbalist’s Crest" });

            AddItem(new WildStaff() { Hue = 1447, Name = "Rootstaff of the Deep Vein" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1165;
            backpack.Name = "Satchel of Rare Herbs";
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
