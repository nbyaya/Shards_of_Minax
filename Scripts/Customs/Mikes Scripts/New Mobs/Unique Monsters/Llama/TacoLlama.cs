using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a taco llama corpse")]
    public class TacoLlama : BaseCreature
    {
        private DateTime m_NextTacoToss;
        private DateTime m_NextHotSauceSpit;
        private DateTime m_NextFiestaFeast;
        private DateTime m_NextFiestaFrenzy;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TacoLlama()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Taco Llama";
            Body = 0xDC; // Llama body
            Hue = 2151; // Unique hue (can be adjusted for a fiesta theme)
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

        public TacoLlama(Serial serial)
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
                    m_NextTacoToss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextHotSauceSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFiestaFeast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFiestaFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTacoToss)
                {
                    TacoToss();
                }

                if (DateTime.UtcNow >= m_NextHotSauceSpit)
                {
                    HotSauceSpit();
                }

                if (DateTime.UtcNow >= m_NextFiestaFeast)
                {
                    FiestaFeast();
                }

                if (DateTime.UtcNow >= m_NextFiestaFrenzy)
                {
                    FiestaFrenzy();
                }
            }
        }

        private void TacoToss()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Taco Llama throws a spicy taco! *");
            PlaySound(0x1F9); // Taco Toss sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 30);

                    // Safe casting to Mobile
                    Mobile target = m as Mobile;
                    if (target != null)
                    {
                        target.OnDamage(damage, this, false); // Provide willKill parameter

                        target.SendMessage("You are hit by a spicy taco explosion!");
                        Effects.SendLocationEffect(target.Location, target.Map, 0x36D4, 16, 4); // Spicy flame effect

                        // Chance to stun
                        if (Utility.RandomDouble() < 0.25) // 25% chance
                        {
                            target.SendMessage("You are stunned by the explosion!");
                            target.Freeze(TimeSpan.FromSeconds(2));
                        }
                    }
                }
            }

            m_NextTacoToss = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for TacoToss
        }

        private void HotSauceSpit()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Taco Llama spits hot sauce! *");
                PlaySound(0x1F9); // Hot Sauce Spit sound

                // Apply burn and slow effect
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (Combatant != null && !Combatant.Deleted)
                    {
                        int damage = Utility.RandomMinMax(10, 20);

                        // Safe casting to Mobile
                        Mobile target = Combatant as Mobile;
                        if (target != null)
                        {
                            target.OnDamage(damage, this, false); // Provide willKill parameter

                            target.SendMessage("You are burned and feel the heat of the hot sauce!");
                            target.SendMessage("Your vision is blurred by the hot sauce!");
                            // Remove or replace the following line with custom effect code if needed
                            // target.SendPropertyChange("HotSauceBlind", true);
                        }
                    }
                });

                m_NextHotSauceSpit = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for HotSauceSpit
            }
        }

        private void FiestaFeast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Taco Llama enjoys a fiesta feast! *");
            PlaySound(0x1F9); // Fiesta Feast sound

            // Heal over time effect
            this.Hits = Math.Min(this.Hits + 30, this.HitsMax);
            m_NextFiestaFeast = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for FiestaFeast

            // Drop tacos more frequently after feast
            if (Utility.RandomDouble() < 0.5) // 50% chance
            {
                TacoItem taco = new TacoItem();
                taco.MoveToWorld(this.Location, this.Map);
            }
        }

        private void FiestaFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Taco Llama enters a fiesta frenzy! *");
            PlaySound(0x1F9); // Fiesta Frenzy sound

            // Temporary damage boost and increased attack speed
            this.SetDamage(this.DamageMin + 10, this.DamageMax + 10);
            this.VirtualArmor -= 10;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.SetDamage(this.DamageMin - 10, this.DamageMax - 10);
                this.VirtualArmor += 10;
            });

            m_NextFiestaFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for FiestaFrenzy
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // Drop tacos more frequently
            if (Utility.RandomDouble() < 0.2) // 20% chance
            {
                TacoItem taco = new TacoItem();
                taco.MoveToWorld(this.Location, this.Map);
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

    public class TacoItem : Item
    {
        public TacoItem() : base(0x171D) // Taco graphic
        {
            Movable = false;
        }

        public TacoItem(Serial serial) : base(serial)
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
