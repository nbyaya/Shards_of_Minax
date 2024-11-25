using System;
using Server.Items;
using Server.Network;
// Add more namespaces if needed

namespace Server.Mobiles
{
    [CorpseName("corpse of a musical giant")]
    public class JazzAgeJuggernaut : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(15.0);
        private DateTime m_NextSpeechTime;
        private DateTime m_NextQuakeTime;
        
        [Constructable]
        public JazzAgeJuggernaut() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "JazzAgeJuggernaut";
            Body = 0x1; // Ogre body, you might want to replace this with the appropriate giant body
			Team = Utility.RandomMinMax(1, 5);
            
            Item saxophone = new Doublet(Utility.RandomRedHue()); // Placeholder, replace with a saxophone item if available
            saxophone.Name = "Enchanted Saxophone";
            AddItem(saxophone);
            
			SetStr( 800, 1200 );
			SetDex( 177, 255 );
			SetInt( 151, 250 );

			SetHits( 600, 1000 );

			SetDamage( 10, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Meditation, 25.1, 50.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
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
            m_NextQuakeTime = DateTime.Now + TimeSpan.FromSeconds(30.0);
        }

        public override void OnThink()
        {
            base.OnThink();
            
            if (DateTime.Now >= m_NextSpeechTime)
            {
                this.Say(true, "Feel the rhythm!");
                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
            
            if (DateTime.Now >= m_NextQuakeTime)
            {
                this.Say("Listen to my powerful tune!");
                Earthquake();
                m_NextQuakeTime = DateTime.Now + TimeSpan.FromMinutes(1.0);
            }
        }

        private void Earthquake()
        {
            Map map = this.Map;

            if (map == null)
                return;

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m.Player || (m is BaseCreature && ((BaseCreature)m).ControlMaster != null))
                {
                    double damage = Utility.RandomMinMax(10, 40);

                    if (m.Player && m.Alive)
                        m.SendLocalizedMessage(1008094); // An earthquake strikes all around you!

                    AOS.Damage(m, this, Convert.ToInt32(damage), 100, 0, 0, 0, 0);
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGold(600, 800);
            // Add more loot here
        }

        public JazzAgeJuggernaut(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
