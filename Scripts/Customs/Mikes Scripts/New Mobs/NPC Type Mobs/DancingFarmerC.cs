using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a dancing farmer")]
    public class DancingFarmerC : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public DancingFarmerC() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // Human male
            Name = "Dancing Farmer";

            AddItem(new Shoes(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Shirt(Utility.RandomNeutralHue()));

            SetStr(100, 200);
            SetDex(100, 200);
            SetInt(100, 200);

            SetHits(100, 200);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Magery, 60.1, 80.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 60.1, 80.0);

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
                        case 0: this.Say(true, "Dance with me!"); break;
                        case 1: this.Say(true, "Can you feel the rhythm?"); break;
                        case 2: this.Say(true, "My feet move to the music!"); break;
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
                case 0: this.Say(true, "The dance ends..."); break;
                case 1: this.Say(true, "The fields will remember my performance..."); break;
            }

            // Summon cows on death
            for (int i = 0; i < Utility.RandomMinMax(1, 3); ++i)
            {
                BaseCreature cow = new Cow();
                cow.MoveToWorld(this.Location, this.Map);
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
                        case 0: this.Say(true, "Watch the dance!"); break;
                        case 1: this.Say(true, "You can't resist the rhythm!"); break;
                        case 2: this.Say(true, "The dance won't be interrupted!"); break;
                        case 3: this.Say(true, "The rhythm beats in my heart!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public DancingFarmerC(Serial serial) : base(serial)
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
