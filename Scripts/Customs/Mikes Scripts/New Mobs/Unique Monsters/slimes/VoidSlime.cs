using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a void slime corpse")]
    public class VoidSlime : BaseCreature
    {
        private DateTime m_NextVoidDrain;
        private DateTime m_NextDimensionalShift;
        private DateTime m_NextEntropicAura;
        private DateTime m_NextVoidBurst;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VoidSlime()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a void slime";
            Body = 51; // Slime body
            Hue = 2379; // Unique hue for Void Slime
			BaseSoundID = 456;

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

            m_AbilitiesInitialized = false;
        }

        public VoidSlime(Serial serial)
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
                    m_NextVoidDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDimensionalShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextEntropicAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30));
                    m_NextVoidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(25, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextVoidDrain)
                {
                    VoidDrain();
                }

                if (DateTime.UtcNow >= m_NextDimensionalShift)
                {
                    DimensionalShift();
                }

                if (DateTime.UtcNow >= m_NextEntropicAura)
                {
                    EntropicAura();
                }

                if (DateTime.UtcNow >= m_NextVoidBurst)
                {
                    VoidBurst();
                }
            }
        }

        private void VoidDrain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Slime releases a draining aura! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int healthDrain = Utility.RandomMinMax(10, 20);
                    int manaDrain = Utility.RandomMinMax(10, 20);

                    AOS.Damage(m, this, healthDrain, 0, 0, 100, 0, 0);
                    m.Mana -= manaDrain;
                    m.SendMessage("You feel your health and mana draining away!");
                }
            }

            m_NextVoidDrain = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void DimensionalShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Slime shifts through dimensions, evading attacks! *");
            PlaySound(0x1FC); // Teleport sound

            Point3D newLocation = new Point3D(
                Location.X + Utility.RandomMinMax(-5, 5),
                Location.Y + Utility.RandomMinMax(-5, 5),
                Location.Z
            );
            if (Map != null && Map.CanFit(newLocation, 16, true))
            {
                Location = newLocation;
            }

            m_NextDimensionalShift = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void EntropicAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Slime's aura disrupts the energies around it! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("The Void Slime's aura makes it harder for you to regenerate health and mana!");
                    // Apply debuff to reduce health and mana regeneration
                    // This can be customized or enhanced
                }
            }

            m_NextEntropicAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void VoidBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Void Slime releases a burst of void energy! *");
            PlaySound(0x1F3); // Burst sound

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Burst effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by a burst of void energy!");
                }
            }

            m_NextVoidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(40);
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

            m_AbilitiesInitialized = false;
        }
    }
}
