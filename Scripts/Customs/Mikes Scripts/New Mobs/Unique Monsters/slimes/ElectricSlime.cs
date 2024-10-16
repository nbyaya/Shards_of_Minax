using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an electric slime corpse")]
    public class ElectricSlime : BaseCreature
    {
        private DateTime m_NextElectricBurst;
        private DateTime m_NextStaticCharge;
        private bool m_IsCharged;
        private const int ChargeDuration = 30; // Duration in seconds to fully charge
        private const int ChargeCooldown = 60; // Cooldown time for static charge

        [Constructable]
        public ElectricSlime()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an electric slime";
            Body = 51; // Slime body
            Hue = 2393; // Electric blue hue
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

            m_NextElectricBurst = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Initial delay for Electric Burst
            m_NextStaticCharge = DateTime.UtcNow + TimeSpan.FromSeconds(ChargeDuration); // Initial delay for Static Charge
            m_IsCharged = false;
        }

        public ElectricSlime(Serial serial)
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
                if (DateTime.UtcNow >= m_NextElectricBurst)
                {
                    ElectricBurst();
                }

                if (DateTime.UtcNow >= m_NextStaticCharge)
                {
                    StaticCharge();
                }
            }
        }

        private void ElectricBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Electric Slime releases a burst of electricity! *");
            PlaySound(0x29F); // Electric sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0); // Electric damage
                    m.SendMessage("You are struck by a burst of electricity!");

                    // Custom stun effect
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => 
                    {
                        if (m.Alive && m.InRange(this, 5))
                        {
                            m.SendMessage("You are stunned!");
                            // Implement stun effect here
                        }
                    });

                    // Chain lightning effect
                    foreach (Mobile nearby in GetMobilesInRange(3))
                    {
                        if (nearby != this && nearby != m && nearby.Alive && CanBeHarmful(nearby))
                        {
                            AOS.Damage(nearby, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Additional damage
                            nearby.SendMessage("You are hit by a surge of electric energy!");
                        }
                    }
                }
            }

            m_NextElectricBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Electric Burst
        }

        private void StaticCharge()
        {
            if (!m_IsCharged)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Electric Slime is charging up with electricity! *");
                PlaySound(0x29F); // Electric sound

                FixedParticles(0x3709, 10, 30, 0, EffectLayer.Waist); // Charging visual effect

                Timer.DelayCall(TimeSpan.FromSeconds(ChargeDuration), () => ReleaseStaticCharge());
                m_NextStaticCharge = DateTime.UtcNow + TimeSpan.FromSeconds(ChargeCooldown); // Cooldown for Static Charge
                m_IsCharged = true;
            }
        }

        private void ReleaseStaticCharge()
        {
            if (m_IsCharged)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Electric Slime releases a massive electric shock! *");
                PlaySound(0x29F); // Electric sound

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0); // Electric damage
                        m.SendMessage("You are hit by a powerful electric shock!");

                        // Custom stun effect
                        Timer.DelayCall(TimeSpan.FromSeconds(4), () => 
                        {
                            if (m.Alive && m.InRange(this, 5))
                            {
                                m.SendMessage("You are stunned!");
                                // Implement stun effect here
                            }
                        });
                    }
                }

                // Create electrical hazards
                for (int i = 0; i < 10; i++) // Create 10 electrical hazards
                {
                    int x = Utility.RandomMinMax(-3, 3);
                    int y = Utility.RandomMinMax(-3, 3);
                    Point3D point = new Point3D(X + x, Y + y, Z);
                    if (InRange(point, 3))
                    {
                        Effects.SendLocationEffect(point, Map, 0x36BD, 30, 10); // Electric spark effect
                    }
                }

                m_IsCharged = false; // Reset charge state
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // Shocking Grasp effect: Chance to stun on melee attack
            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Electric Slime's shocking grasp stuns you! *");
                PlaySound(0x29F); // Electric sound
                
                // Custom stun effect
                Timer.DelayCall(TimeSpan.FromSeconds(2), () => 
                {
                    if (from.Alive && from.InRange(this, 1))
                    {
                        from.SendMessage("You are stunned!");
                        // Implement stun effect here
                    }
                });
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
