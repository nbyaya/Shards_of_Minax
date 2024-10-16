using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a mariachi llama corpse")]
    public class ElMariachiLlama : BaseCreature
    {
        private DateTime m_NextSonicSerenade;
        private DateTime m_NextMusicOfHealing;
        private DateTime m_NextDramaticEncore;
        private DateTime m_NextFieryFinale;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ElMariachiLlama()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "El Mariachi Llama";
            Body = 0xDC; // Llama body
            Hue = 1155; // Unique hue for El Mariachi Llama
			this.BaseSoundID = 0x3F3;

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

        public ElMariachiLlama(Serial serial)
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
                    m_NextSonicSerenade = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextMusicOfHealing = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDramaticEncore = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFieryFinale = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 15));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSonicSerenade)
                {
                    SonicSerenade();
                }

                if (DateTime.UtcNow >= m_NextMusicOfHealing)
                {
                    MusicOfHealing();
                }

                if (DateTime.UtcNow >= m_NextDramaticEncore)
                {
                    DramaticEncore();
                }

                if (DateTime.UtcNow >= m_NextFieryFinale)
                {
                    m_NextFieryFinale = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)); // Re-cooldown FieryFinale
                }
            }
        }

        private void SonicSerenade()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* El Mariachi Llama plays a powerful tune that stuns enemies! *");
            PlaySound(0x1B6); // Sound of music

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    if (CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        m.SendMessage("You are stunned by the llama's powerful serenade!");
                        m.Freeze(TimeSpan.FromSeconds(3));
                        Effects.SendTargetEffect(m, 0x36B0, 10); // Visual effect for stun
                    }
                }
            }

            m_NextSonicSerenade = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for SonicSerenade
        }

        private void MusicOfHealing()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* El Mariachi Llama plays a healing tune! *");
            PlaySound(0x1B6); // Sound of music

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && creature.Alive)
                {
                    int healAmount = Utility.RandomMinMax(10, 20);
                    creature.Hits += healAmount;
                    m.SendMessage($"You feel the soothing music heal you for {healAmount} HP!");
                }
            }

            m_NextMusicOfHealing = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for MusicOfHealing
        }

        private void DramaticEncore()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* El Mariachi Llama performs a dramatic encore! *");
            PlaySound(0x1B6); // Sound of music

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(5, 15);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Music damage
                    m.SendMessage($"You are hit by the dramatic encore, taking {damage} damage!");
                }
            }

            m_NextDramaticEncore = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for DramaticEncore
        }

        private void FieryFinale()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* El Mariachi Llama plays a final explosive tune! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Fire explosion effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0); // Fire damage
                    m.SendMessage("You are hit by the explosive final tune!");
                    m.PlaySound(0x1DD); // Explosion sound
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            FieryFinale();
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
