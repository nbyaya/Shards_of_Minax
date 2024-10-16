using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a dancing farmer")]
    public class DancingFarmerB : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public DancingFarmerB() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // Human male
            Name = "Dancing Farmer";

            AddItem(new StrawHat());
            AddItem(new Shirt(Utility.RandomBlueHue()));
            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            AddItem(new Boots());

            SetStr(100, 200);
            SetDex(50, 75);
            SetInt(25, 50);

            SetHits(50, 100);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Magery, 60.1, 80.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 60.1, 80.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 1000;
            Karma = 1000;

            VirtualArmor = 20;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Dance with me, traveler!"); break;
                        case 1: this.Say(true, "Can you hear the rhythm of the fields?"); break;
                        case 2: this.Say(true, "My feet move to the music of the earth!"); break;
                        case 3: this.Say(true, "The fields sing with me!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(50, 100);
            AddLoot(LootPack.Average);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The fields will remember my dance..."); break;
                case 1: this.Say(true, "The song ends..."); break;
            }

            // Summon cows when the farmer dies
            for (int i = 0; i < Utility.RandomMinMax(1, 3); ++i)
            {
                SummonCows(3); // Summon 3 cows upon death
            }
        }
		
		private void SummonCows(int numberOfCows)
        {
            for (int i = 0; i < numberOfCows; i++)
            {
                Cow cow = new Cow
                {
                    Controlled = true, // Assuming you want the cows to be friendly/controlled
                    ControlMaster = this.SummonMaster, // Set to the farmer or another entity if desired
                    Home = this.Location, // The cow's home is set to where the farmer died
                    RangeHome = 10, // How far the cow can wander from its home
                };

                cow.MoveToWorld(this.Location, this.Map); // Place the cow in the world at the farmer's location
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Watch the dance of the scythe!"); break;
                        case 1: this.Say(true, "You can't resist the farmer's jig!"); break;
                        case 2: this.Say(true, "The dance of the fields won't be interrupted!"); break;
                        case 3: this.Say(true, "The earth's rhythm beats in my heart!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public DancingFarmerB(Serial serial) : base(serial)
        {
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
