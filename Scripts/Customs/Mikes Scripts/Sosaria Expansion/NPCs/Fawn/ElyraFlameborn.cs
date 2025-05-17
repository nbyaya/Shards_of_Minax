using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BreakTheChainsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Break the Chains"; } }

        public override object Description
        {
            get
            {
                return
                    "I am Elyra Flameborn, daughter of smoke and scale. My kin remain shackled beneath Drakkon's wrath—overseers with scorched brands ensure their chains never loosen.\n\n" +
                    "Slay **4 Dragonbound Overseers** in the Caves of Drakkon. Each one you slay weakens Drakkon’s grip, and strengthens our chance at freedom.";
            }
        }

        public override object Refuse { get { return "Then their chains remain unbroken. May the fire judge your inaction."; } }

        public override object Uncomplete { get { return "The Overseers still torment my people. I feel it in my blood."; } }

        public override object Complete { get { return "The flames rejoice. You’ve begun what I cannot finish. Here—take this. A token of our pact."; } }

        public BreakTheChainsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DragonboundOverseer), "Dragonbound Overseers", 4));

            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(DraconicEmber), 1, "Draconic Ember (Quest Relic)"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Break the Chains'!");
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

    public class ElyraFlameborn : MondainQuester
    {
        
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this)); 
        }
		
		[Constructable]
        public ElyraFlameborn()
            : base("Elyra Flameborn", "Exiled Dragonblood")
        {
        }

        public ElyraFlameborn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(90, 85, 100);

            Female = true;
            Race = Race.Human;

            Body = 0x191;
            Name = "Elyra Flameborn";
            Hue = 1153; // Subtle reddish hue

            HairItemID = 0x2049;
            HairHue = 1359;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(33)); // Deep crimson robe
            AddItem(new LeatherGloves());
            AddItem(new Sandals());

            AddItem(new Cloak(1157)); // Flame-like hue
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BreakTheChainsQuest)
                };
            }
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
