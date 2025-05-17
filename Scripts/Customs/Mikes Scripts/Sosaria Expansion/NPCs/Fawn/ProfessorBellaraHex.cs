using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SpecimensOfTheArcaneQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Specimens of the Arcane";

        public override object Description => 
            "Ahh, a curious mind—or just a brave fool? Either suits me.\n\n" +
            "The faculty at Malidor’s Academy—those cowards—banished me for daring to ask real questions. " +
            "But now, the rogue energies leaking from their sacred halls have begun to take shape—*life*, if you can call it that.\n\n" +
            "Bring me three **Living Arcane Essences**, harvested from the beasts that now wander the Academy. They pulse with unstable mana—raw, magnificent potential!";

        public override object Refuse =>
            "Tch. Typical. If you lack vision, perhaps stick to baking bread in Dawn.";

        public override object Uncomplete =>
            "Still empty-handed? Don’t tell me the Academy’s hounds frightened you. They are but toddlers of the arcane. Barely aware. Mostly teeth.";

        public override object Complete => 
            "Exquisite! These are *alive*—do you feel the heat in your palm? The pulse? The judgment?\n\n" +
            "You’ve proven yourself more than a glorified errand-runner. Perhaps... yes, perhaps I could teach you a thing or two—things they locked away, afraid of their own power.\n\n" +
            "But that comes later. For now, take this. Payment, and promise.";

        public SpecimensOfTheArcaneQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(LivingArcaneEssence), "Living Arcane Essence", 3, 0x3188, 1153));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(BlasphemousGrimoire), 1, "Tome of Withheld Knowledge"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Specimens of the Arcane quest!");
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

    public class ProfessorBellaraHex : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage()); 
        }        
		
		[Constructable]
        public ProfessorBellaraHex() : base("Disgraced Spellwright", "Professor Bellara Hex")
        {
        }

        public override void InitBody()
        {
            InitStats(90, 80, 100);
            Body = 401;
            Hue = 2204;
            HairItemID = 0x203D;
            HairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1153));
            AddItem(new Sandals(0));
            AddItem(new WizardsHat(1153));
            AddItem(new Spellbook { Name = "Hexwork Journal", Hue = 1153 });

            Backpack pack = new Backpack();
            pack.Name = "Spellwright’s Satchel";
            AddItem(pack);
        }

        public override Type[] Quests => new Type[]
        {
            typeof(SpecimensOfTheArcaneQuest)
        };

        public ProfessorBellaraHex(Serial serial) : base(serial) { }

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
