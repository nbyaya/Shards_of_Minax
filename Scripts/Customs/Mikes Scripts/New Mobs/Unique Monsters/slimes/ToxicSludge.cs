using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a toxic sludge corpse")]
    public class ToxicSludge : BaseCreature
    {
        private DateTime m_NextToxicCloud;
        private DateTime m_NextCorrosiveTrail;
        private DateTime m_NextSlimeSplash;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ToxicSludge()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Toxic Sludge";
            Body = 51; // Slime body
            Hue = 2378; // Unique hue
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

        public ToxicSludge(Serial serial)
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
                    m_NextToxicCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCorrosiveTrail = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextSlimeSplash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextToxicCloud)
                {
                    ToxicCloud();
                }

                if (DateTime.UtcNow >= m_NextCorrosiveTrail)
                {
                    CorrosiveTrail();
                }

                if (DateTime.UtcNow >= m_NextSlimeSplash)
                {
                    SlimeSplash();
                }
            }
        }

        private void ToxicCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Sludge releases a noxious cloud of poison! *");
            PlaySound(0x20B); // Toxic sound effect

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Cloud effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    ApplyPoison(m);
                }
            }

            m_NextToxicCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ToxicCloud
        }

        private void CorrosiveTrail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Sludge leaves a trail of corrosive sludge! *");
            PlaySound(0x20C); // Corrosive sound effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    ApplyCorrosiveDamage(m);
                }
            }

            m_NextCorrosiveTrail = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for CorrosiveTrail
        }

        private void ApplyCorrosiveDamage(Mobile target)
        {
            if (CanBeHarmful(target))
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 0, 0); // Corrosive damage
                target.SendMessage("You are burned by the corrosive trail!");
                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 10, 4); // Corrosive effect
                target.SendMessage("You feel your skin burning from the corrosive sludge!");
            }
        }

        private void SlimeSplash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Sludge splashes acidic slime at its foes! *");
            PlaySound(0x20D); // Splash sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    ApplySlimeSplash(m);
                }
            }

            m_NextSlimeSplash = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for SlimeSplash
        }

        private void ApplySlimeSplash(Mobile target)
        {
            if (CanBeHarmful(target))
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(15, 20), 0, 0, 0, 0, 0); // Acidic damage
                target.SendMessage("You are hit by a splash of acidic slime!");
                Effects.SendLocationEffect(target.Location, target.Map, 0x373A, 10, 3); // Splash effect
                target.SendMessage("You feel your vision blur from the acidic slime!");
                target.FixedParticles(0x373A, 1, 30, 9944, EffectLayer.Waist); // Visual effect
            }
        }

        private void ApplyPoison(Mobile target)
        {
            if (CanBeHarmful(target))
            {
                DoHarmful(target);
                Poison poison = Poison.Lethal; // Severe poison
                target.ApplyPoison(this, poison);
                target.SendMessage("You are engulfed by the toxic cloud!");
            }
        }

        public override void OnDamage(int amount, Mobile attacker, bool willKill)
        {
            base.OnDamage(amount, attacker, willKill);

            if (willKill || attacker == null || !CanBeHarmful(attacker))
                return;

            if (Utility.RandomDouble() < 0.25) // 25% chance to infect attacker
            {
                ApplyContagion(attacker);
            }
        }

        private void ApplyContagion(Mobile attacker)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Sludge's disease infects its attacker! *");
            attacker.SendMessage("You feel a sudden illness overtaking you!");

            // Reduce stats and other effects temporarily
            attacker.RawStr -= 10;
            attacker.RawDex -= 10;
            attacker.RawInt -= 10;
            attacker.SendMessage("Your physical and mental abilities are weakened!");

            // Apply a debuff or a negative effect to simulate movement speed reduction
            // Example: attacker.AddToBackpack(new DebuffItem()); // This is just illustrative
            
            Timer.DelayCall(TimeSpan.FromSeconds(30), () => 
            {
                // Restore stats and effects after 30 seconds
                if (attacker != null && !attacker.Deleted)
                {
                    attacker.RawStr += 10;
                    attacker.RawDex += 10;
                    attacker.RawInt += 10;
                    // Restore any other temporary effects
                }
            });
        }

        public override bool OnBeforeDeath()
        {
            // Call the base method and return its result
            bool result = base.OnBeforeDeath();

            // Perform additional actions
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Sludge collapses into a pool of putrid liquid! *");

            return result; // Ensure the base behavior is preserved
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
