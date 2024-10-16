using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server;

namespace Server.Mobiles
{
    [CorpseName("a cursed wolf corpse")]
    public class CursedWolf : BaseCreature
    {
        private DateTime m_NextSoulDrain;
        private DateTime m_NextCursedAura;
        private DateTime m_NextHex;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public CursedWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed wolf";
            Body = 23; // DireWolf body
            Hue = 2637; // Unique hue (dark, cursed look)
			BaseSoundID = 0xE5;
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public CursedWolf(Serial serial)
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
                    m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHex = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSoulDrain)
                {
                    SoulDrain();
                }

                if (DateTime.UtcNow >= m_NextCursedAura)
                {
                    CursedAura();
                }

                if (DateTime.UtcNow >= m_NextHex)
                {
                    Hex();
                }
            }
        }

        private void SoulDrain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(5, 10);
                target.Damage(damage, this);
                Hits = Math.Min(Hits + damage, HitsMax);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cursed Wolf drains souls! *");
                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for SoulDrain
            }
        }

        private void CursedAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cursed Aura spreads! *");
            PlaySound(0x20F);
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player && m.Alive)
                {
                    // Apply debuff effect (reduce resistances)
                    m.SendMessage("You feel the Cursed Aura weakening you!");
                    m.CantWalk = true; // Just a placeholder for actual debuff effects
                }
            }

            m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for CursedAura
        }

        private void Hex()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int hexEffect = Utility.Random(3); // Randomly choose an effect
                switch (hexEffect)
                {
                    case 0:
                        target.SendMessage("You are cursed with reduced damage output!");
                        // Apply damage reduction effect
                        break;
                    case 1:
                        target.SendMessage("You are cursed with slowed movement!");
                        // Apply movement slow effect
                        break;
                    case 2:
                        target.SendMessage("You are cursed with reduced attack speed!");
                        // Apply attack speed reduction effect
                        break;
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cursed Hex! *");
                m_NextHex = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Hex
            }
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
