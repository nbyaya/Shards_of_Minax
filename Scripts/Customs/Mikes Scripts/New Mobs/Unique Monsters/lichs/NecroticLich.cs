using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a necrotic lich corpse")]
    public class NecroticLich : BaseCreature
    {
        private DateTime m_NextDecayAura;
        private DateTime m_NextCorpseExplosion;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NecroticLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a necrotic lich";
            Body = 24; // Lich body
            Hue = 2136; // Unique hue for the Necrotic Lich
            BaseSoundID = 0x3E9;

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
            ControlSlots = 3;
            MinTameSkill = 93.9;

            PackNecroReg(20, 30);

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public NecroticLich(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCorpseExplosion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDecayAura)
                {
                    DecayAura();
                }

                if (DateTime.UtcNow >= m_NextCorpseExplosion)
                {
                    CorpseExplosion();
                }
            }
        }

        private void DecayAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Necrotic Decay Aura! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    int damage = (int)(m.Hits * 0.05); // 5% of current HP
                    m.Damage(damage, this);
                    m.SendMessage("The Decay Aura of the Necrotic Lich weakens you!");
                }
            }

            m_NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)); // Random cooldown between 10 and 20 seconds
        }

        private void CorpseExplosion()
        {
            foreach (Item item in GetItemsInRange(5))
            {
                if (item is Corpse corpse && corpse.Items.Count > 0)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Corpse Explosion! *");
                    FixedEffect(0x374A, 10, 16);

                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Player)
                        {
                            int damage = Utility.RandomMinMax(10, 20);
                            m.Damage(damage);
                            m.SendMessage("You are hit by a blast of poison from a corpse!");
                        }
                    }

                    corpse.Delete();
                    m_NextCorpseExplosion = DateTime.UtcNow + TimeSpan.FromMinutes(Utility.RandomMinMax(1, 2)); // Random cooldown between 1 and 2 minutes
                    break;
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Drop some necromancy-related items or gold
            c.DropItem(new BonePile());
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
