using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mud golem corpse")]
    public class MudGolem : BaseCreature
    {
        private DateTime m_NextMudTrap;
        private DateTime m_NextMudBomb;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MudGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a mud golem";
            Body = 14;
            BaseSoundID = 268;
            Hue = 1501; // Muddy brown hue

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

            PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public MudGolem(Serial serial)
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

        public override bool BleedImmune { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextMudTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMudBomb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMudTrap)
                {
                    MudTrap();
                }

                if (DateTime.UtcNow >= m_NextMudBomb)
                {
                    MudBomb();
                }
            }
        }

        private void MudTrap()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Creates a pool of sticky mud *");
            PlaySound(0x22F);
            FixedEffect(0x376A, 10, 15);

            List<Mobile> targets = new List<Mobile>(); // Initialize the list here
            IPooledEnumerable eable = GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m); // Add mobiles to the targets list
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                m.SendLocalizedMessage(1072124, "", 0x206); // You are mired in sticky mud, slowing your movement.
                m.FixedParticles(0x37CC, 1, 10, 0x1F78, 0x496, 0, EffectLayer.Waist);
                int oldDex = m.Dex;
                m.Dex = (int)(m.Dex * 0.5);

                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    m.Dex = oldDex;
                    m.SendLocalizedMessage(1072060, "", 0x206); // You manage to free yourself from the mud.
                });
            }

            m_NextMudTrap = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
        }

        private void MudBomb()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Throws a glob of mud *");
            PlaySound(0x145);

            if (Combatant is Mobile target)
            {
                Direction = GetDirectionTo(target);
                MovingEffect(target, 0xF0D, 7, 1, false, false);

                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (target.Alive)
                    {
                        target.Freeze(TimeSpan.FromSeconds(3));
                        target.FixedEffect(0x376A, 9, 32);
                        target.PlaySound(0x201);
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 100, 0, 0, 0, 0);
                    }
                });
            }

            m_NextMudBomb = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset cooldown
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
