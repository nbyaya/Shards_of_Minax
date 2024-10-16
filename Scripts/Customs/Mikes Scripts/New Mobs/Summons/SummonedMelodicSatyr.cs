using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a melodic satyr's corpse")]
    public class SummonedMelodicSatyr : BaseCreature
    {
        private DateTime m_NextHealingSong;
        private DateTime m_NextCalmingChorus;
        private DateTime m_NextHarmoniousAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedMelodicSatyr()
            : base(AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a melodic satyr";
            Body = 271; // Satyr body
            Hue = 2322; // Custom hue for the Melodic Satyr
			this.BaseSoundID = 0x586;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedMelodicSatyr(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextHealingSong = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCalmingChorus = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextHarmoniousAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHealingSong)
                {
                    HealingSong();
                }

                if (DateTime.UtcNow >= m_NextCalmingChorus)
                {
                    CalmingChorus();
                }

                if (DateTime.UtcNow >= m_NextHarmoniousAura)
                {
                    HarmoniousAura();
                }
            }
        }

        private void HealingSong()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a soothing melody to heal allies *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && IsAlly(m))
                {
                    m.Hits += 10; // Healing amount
                    m.SendMessage("You feel rejuvenated by the Melodic Satyr's melody.");
                }
            }

            m_NextHealingSong = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for HealingSong
        }

        private void CalmingChorus()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sings a calming chorus *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && IsEnemy(m))
                {
                    // Lowering attack speed and damage
                    if (m is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        bc.SetDamageType(ResistanceType.Physical, 50);
                        bc.SetDamageType(ResistanceType.Poison, 50);
                        m.SendMessage("You feel your aggression lessen.");
                    }
                }
            }

            m_NextCalmingChorus = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for CalmingChorus
        }

        private void HarmoniousAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Emits a harmonious aura *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && IsAlly(m))
                {
                    ((BaseCreature)m).VirtualArmor += 10; // Defense boost
                    m.SendMessage("You feel more protected by the Melodic Satyr's aura.");
                }
            }

            m_NextHarmoniousAura = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for HarmoniousAura
        }

        private bool IsAlly(Mobile mobile)
        {
            return mobile != null && (mobile is PlayerMobile || (mobile is BaseCreature && !((BaseCreature)mobile).IsEnemy(this)));
        }

        private bool IsEnemy(Mobile mobile)
        {
            return mobile != null && (mobile is BaseCreature && ((BaseCreature)mobile).IsEnemy(this));
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
