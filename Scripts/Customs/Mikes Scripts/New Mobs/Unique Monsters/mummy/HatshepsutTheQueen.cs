using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a queen's corpse")]
    public class HatshepsutTheQueen : BaseCreature
    {
        private DateTime m_NextRoyalDecree;
        private DateTime m_NextAncientKnowledge;
        private DateTime m_NextSummonGuards;
        private DateTime m_NextCursedAura;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public HatshepsutTheQueen()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Hatshepsut the Queen";
            Body = 154; // Mummy body
            Hue = 2165; // Unique hue (you can choose another if desired)
			BaseSoundID = 471;

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
        }

        public HatshepsutTheQueen(Serial serial)
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
                    m_NextRoyalDecree = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAncientKnowledge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonGuards = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRoyalDecree)
                {
                    RoyalDecree();
                }

                if (DateTime.UtcNow >= m_NextAncientKnowledge)
                {
                    AncientKnowledge();
                }

                if (DateTime.UtcNow >= m_NextSummonGuards)
                {
                    SummonGuards();
                }

                if (DateTime.UtcNow >= m_NextCursedAura)
                {
                    CursedAura();
                }
            }
        }

        private void RoyalDecree()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Hatshepsut the Queen issues a Royal Decree! *");
            PlaySound(0x1F4); // Royal sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature)
                {
                    // Increase stats and grant a special ability
                    

                    // Special ability: temporary increased damage


                    // Visual effect: royal aura
                    FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                }
            }

            m_NextRoyalDecree = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for RoyalDecree
        }

        private void AncientKnowledge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Hatshepsut the Queen invokes Ancient Knowledge! *");
            PlaySound(0x1F5); // Knowledge sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature)
                {
                    // Heal and restore mana
                    m.Heal(40);
                    m.Mana += 40;

                    // Defensive buff: increase resistances temporarily
                    m.RawStr += 10;
                    m.RawDex += 10;
                    m.RawInt += 10;

                    // Visual effect: radiant light
                    FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                }
            }

            m_NextAncientKnowledge = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for AncientKnowledge
        }

        private void SummonGuards()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Hatshepsut the Queen summons her royal guards! *");
            PlaySound(0x1F6); // Summon sound

            // Summon 2-4 Mummy guards
            for (int i = 0; i < Utility.RandomMinMax(2, 4); i++)
            {
                BaseCreature guard = new Mummy(); // You can create a special type of guard if needed
                guard.MoveToWorld(Location, Map);
                guard.Combatant = Combatant;
            }

            m_NextSummonGuards = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for SummonGuards
        }

        private void CursedAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Hatshepsut the Queen emits a Cursed Aura! *");
            PlaySound(0x1F7); // Cursed sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    // Apply debuff
                    m.RawStr -= 10;
                    m.RawDex -= 10;
                    m.RawInt -= 10;
                    m.VirtualArmor -= 10;

                    // Visual effect: cursed particles
                    MovingParticles(m, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);
                }
            }

            m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for CursedAura
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
