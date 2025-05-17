using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LostAndFoundQuestA : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Lost and Found"; } }

        public override object Description
        {
            get
            {
                return
                    "Ophis Brasscoil, a historian cloaked in brass and mystery, regards you through lenses tinted with crimson hues.\n\n" +
                    "\"The mines whisper, adventurer. They speak of a shift leader who vanished, yet left behind a map of veins so rich, they shimmer through stone. His diary—ink fading by moonlight—holds secrets we must reclaim.\"\n\n" +
                    "\"Find him, or what's left. The LostMiner roams still, and his journal... only brass binding can keep it whole. Return it, and Devil Guard’s future may yet be bright.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The past fades fast in the dark, but so too will our hopes if that diary stays lost.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You’ve yet to confront the LostMiner? The longer he roams, the deeper his knowledge slips away.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The diary! Brass-bound and fragile... I feel its pulse still. You’ve not only slain a cursed soul—you’ve revived our legacy.\n\n" +
                       "Take this, a *BloodleafTotemMask*. May it guard your mind as steadfastly as you’ve guarded history.";
            }
        }

        public LostAndFoundQuestA() : base()
        {
            AddObjective(new SlayObjective(typeof(LostMiner), "the LostMiner", 1));
            AddReward(new BaseReward(typeof(BloodleafTotemMask), 1, "BloodleafTotemMask"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Lost and Found'!");
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

    public class OphisBrasscoil : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(LostAndFoundQuestA) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this)); // As a historian working with brass and relics
        }

        [Constructable]
        public OphisBrasscoil()
            : base("the Brass Historian", "Ophis Brasscoil")
        {
        }

        public OphisBrasscoil(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 70, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2047; // Long Hair
            HairHue = 1175; // Brass-tinted
            FacialHairItemID = 0x204B; // Long Beard
            FacialHairHue = 1175;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2213, Name = "Brass-Plate Vestment" }); // Brass hue
            AddItem(new StuddedArms() { Hue = 2413, Name = "Historian’s Bracelets" });
            AddItem(new LeatherGloves() { Hue = 2405, Name = "Archivist’s Grip" });
            AddItem(new StuddedLegs() { Hue = 2207, Name = "Relic-Seeker’s Greaves" });
            AddItem(new WizardsHat() { Hue = 1175, Name = "Hat of Gilded Knowledge" });
            AddItem(new Sandals() { Hue = 1819, Name = "Tome-Treader Sandals" });

            AddItem(new Scepter() { Hue = 2500, Name = "Chronicle Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Brassbound Archive Pack";
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
