using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of BeetleJuice")]
    public class BeetleJuiceSummoner : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between BeetleJuice's speech
        private DateTime m_NextSpeechTime;
        
        [Constructable]
        public BeetleJuiceSummoner() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x455; // BeetleJuice's pale hue.
            Body = 0x190; // Male body form.
            Name = "BeetleJuice";
  			Team = Utility.RandomMinMax(1, 5);
          
            // Clothing and appearance
            Item suit = new FancyShirt(Utility.RandomBlueHue());
            Item pants = new LongPants(Utility.RandomBlueHue());
            AddItem(suit);
            AddItem(pants);
            
            // Abilities and stats
            SetStr(800, 1200);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(600, 1000);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Archery, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.ArmsLore, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Bushido, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Chivalry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Fencing, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Lumberjacking, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Ninjitsu, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Parry, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Swords, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Tactics, Utility.RandomMinMax(50, 100));
            SetSkill(SkillName.Wrestling, Utility.RandomMinMax(50, 100));

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 58;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: Say("It's showtime!"); SummonBeetle(); break;
                    case 1: Say("Let's turn on the juice and see what shakes loose!"); SummonBeetle(); break;
                    case 2: Say("I'm the ghost with the most, babe!"); SummonBeetle(); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(3);

                    switch (phrase)
                    {
                        case 0: Say("It's showtime!"); SummonBeetle(); break;
                        case 1: Say("Let's turn on the juice and see what shakes loose!"); SummonBeetle(); break;
                        case 2: Say("I'm the ghost with the most, babe!"); SummonBeetle(); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        public void SummonBeetle()
        {
            BaseCreature creature;

            switch (Utility.Random(3))
            {
                default:
                case 0: creature = new Beetle(); break;
                case 1: creature = new Beetle(); break;
                case 2: creature = new FireBeetle(); break;
            }

            creature.MoveToWorld(this.Location, this.Map);
            creature.Combatant = this.Combatant;
        }

        public override void GenerateLoot()
        {
            PackGold(200, 250);
            PackGem();
            AddLoot(LootPack.Rich);
        }

        public BeetleJuiceSummoner(Serial serial) : base(serial)
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
