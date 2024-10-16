using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a gummy sheep corpse")]
    public class GummySheep : BaseCreature
    {
        private DateTime m_NextGummyGrip;
        private DateTime m_NextCandyBurst;
        private DateTime m_NextGummyShield;
        private DateTime m_GummyShieldEnd;
        private bool m_IsShieldActive;

        [Constructable]
        public GummySheep()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Gummy Sheep";
            Body = 0xCF; // Sheep body
            Hue = 2350; // Unique hue
			BaseSoundID = 0xD6;

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

            m_IsShieldActive = false;
        }

        public GummySheep(Serial serial)
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
                // Check if abilities need to be activated
                if (DateTime.UtcNow >= m_NextGummyGrip)
                {
                    GummyGrip();
                }

                if (DateTime.UtcNow >= m_NextCandyBurst)
                {
                    CandyBurst();
                }

                if (DateTime.UtcNow >= m_NextGummyShield)
                {
                    ActivateGummyShield();
                }
            }

            // Deactivate the shield if it's expired
            if (m_IsShieldActive && DateTime.UtcNow >= m_GummyShieldEnd)
            {
                DeactivateGummyShield();
            }

            // Release sticky goo on the ground periodically
            if (Utility.RandomDouble() < 0.1) // 10% chance per think cycle
            {
                ReleaseStickyGoo();
            }


        }

        private void GummyGrip()
        {
            if (Combatant != null)
            {
                // Ensure Combatant is a Mobile
                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Gummy Sheep squeezes its target with a gummy grip!*");
                    PlaySound(0x1B3); // Gummy sound

                    // Immobilize the target for 5 seconds
                    target.Paralyze(TimeSpan.FromSeconds(5));

                    // Apply a slow effect for 10 seconds afterward
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        if (target != null && target.Alive)
                        {
                            target.SendMessage("You are still slowed down by the gummy grip!");
                            // Replace with actual slow effect implementation if available
                        }
                    });

                    m_NextGummyGrip = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Gummy Grip
                }
            }
        }

        private void CandyBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Gummy Sheep releases a burst of candy shards!*");
            PlaySound(0x1B4); // Candy burst sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0); // Candy shard damage
                    m.SendMessage("You are hit by a burst of candy shards!");

                    // 25% chance to confuse the target
                    if (Utility.RandomDouble() < 0.25)
                    {
                        m.SendMessage("You feel disoriented from the candy burst!");
                        m.Paralyze(TimeSpan.FromSeconds(3)); // Confusion effect
                    }
                }
            }

            m_NextCandyBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Candy Burst
        }

        private void ActivateGummyShield()
        {
            if (!m_IsShieldActive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Gummy Sheep envelops itself in a sticky gummy shield!*");
                PlaySound(0x1B5); // Shield sound

                // Apply a shield effect
                FixedParticles(0x36D4, 9, 32, 5030, EffectLayer.Waist);

                m_GummyShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                m_IsShieldActive = true;

                m_NextGummyShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Gummy Shield
            }
        }

        private void DeactivateGummyShield()
        {
            if (m_IsShieldActive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Gummy Sheep's gummy shield fades away!*");
                m_IsShieldActive = false;
            }
        }

        private void ReleaseStickyGoo()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Gummy Sheep releases sticky goo onto the ground!*");
            PlaySound(0x1B6); // Goo sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Sticky goo damage
                    m.SendMessage("You slip and slide in the sticky goo!");
                }
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
        }
    }
}
