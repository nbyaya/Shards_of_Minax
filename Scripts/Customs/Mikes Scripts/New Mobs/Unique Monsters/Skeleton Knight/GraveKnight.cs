using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of a grave knight")]
    public class GraveKnight : BaseCreature
    {
        private DateTime m_NextGraveCall;
        private bool m_CanResurrect;
        private bool m_AbilitiesInitialized; // Flag to check if abilities are initialized

        [Constructable]
        public GraveKnight()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a grave knight";
            Body = 57; // BoneKnight body
            BaseSoundID = 451;
            Hue = 2370; // Dark red hue

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

            PackItem(new Scimitar());
            PackItem(new PlateChest());
            PackItem(new PlateArms());
            PackItem(new PlateGloves());
            PackItem(new PlateGorget());
            PackItem(new PlateLegs());
            PackItem(new PlateHelm());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public GraveKnight(Serial serial)
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

        public override bool AlwaysMurderer { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextGraveCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGraveCall)
                {
                    DoGraveCall();
                }
            }
        }

        private void DoGraveCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grave Call *");
            PlaySound(0x5C4);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            foreach (Mobile m in targets)
            {
                m.SendLocalizedMessage(1062506); // A wave of bone-chilling cold washes over you.
                m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                m.PlaySound(0x5C5);

                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                m.AddStatMod(new StatMod(StatType.Str, "GraveCall_Str", -10, TimeSpan.FromSeconds(10)));
                m.AddStatMod(new StatMod(StatType.Dex, "GraveCall_Dex", -10, TimeSpan.FromSeconds(10)));
                m.VirtualArmorMod -= 10;

                Timer.DelayCall(TimeSpan.FromSeconds(10), () => m.VirtualArmorMod += 10);
            }

            // Set a new random interval for the next Grave Call
            Random rand = new Random();
            m_NextGraveCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // 50% chance to spawn a new GraveKnight
            if (Utility.RandomDouble() < 0.5)
            {
                GraveKnight newKnight = new GraveKnight();
                newKnight.Team = this.Team; // Ensure it retains the same team if necessary
                newKnight.MoveToWorld(Location, Map);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_CanResurrect);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_CanResurrect = reader.ReadBool();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
