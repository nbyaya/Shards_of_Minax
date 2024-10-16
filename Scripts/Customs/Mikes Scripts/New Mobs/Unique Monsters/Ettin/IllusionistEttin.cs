using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an illusionist ettin's corpse")]
    public class IllusionistEttin : BaseCreature
    {
        private DateTime m_NextMirrorImages;
        private DateTime m_NextPhantomStrike;
        private DateTime m_NextDisorientingGaze;
        private List<Mobile> m_Clones;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IllusionistEttin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an illusionist ettin";
            Body = 18;
            BaseSoundID = 367;
            Hue = 1561; // Mystical blue hue

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

            m_Clones = new List<Mobile>();
            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public IllusionistEttin(Serial serial)
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

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 4; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMirrorImages = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDisorientingGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMirrorImages)
                {
                    CreateMirrorImages();
                }

                if (DateTime.UtcNow >= m_NextPhantomStrike)
                {
                    PhantomStrike();
                }

                if (DateTime.UtcNow >= m_NextDisorientingGaze)
                {
                    DisorientingGaze();
                }
            }
        }

        private void CreateMirrorImages()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Illusionist Ettin creates mirror images! *");
            PlaySound(0x511);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    IllusionaryClone clone = new IllusionaryClone(this);
                    clone.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(EffectItem.Create(clone.Location, clone.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                    m_Clones.Add(clone);
                }
            }

            m_NextMirrorImages = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for MirrorImages
        }

        private void PhantomStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Illusionist Ettin unleashes a phantom strike! *");
            PlaySound(0x231);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                }
            }

            m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for PhantomStrike
        }

        private void DisorientingGaze()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Illusionist Ettin's gaze disorients you! *");
            PlaySound(0x1ED);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    m.SendMessage("You feel disoriented and confused!");
                    m.Paralyze(TimeSpan.FromSeconds(3));

                    StatMod mod = new StatMod(StatType.Dex, "IllusionistEttin_Dex_Curse", -20, TimeSpan.FromSeconds(10));
                    m.AddStatMod(mod);

                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Head);
                }
            }

            m_NextDisorientingGaze = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for DisorientingGaze
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            foreach (IllusionaryClone clone in m_Clones)
            {
                if (!clone.Deleted)
                    clone.Delete();
            }

            m_Clones.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write(m_Clones.Count);
            foreach (IllusionaryClone clone in m_Clones)
                writer.Write(clone);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int count = reader.ReadInt();
            m_Clones = new List<Mobile>();
            for (int i = 0; i < count; i++)
            {
                IllusionaryClone clone = reader.ReadMobile() as IllusionaryClone;
                if (clone != null && !clone.Deleted)
                    m_Clones.Add(clone);
            }

            // Reset ability initialization
            m_AbilitiesInitialized = false;
        }
    }

    public class IllusionaryClone : BaseCreature
    {
        private Mobile m_Master;

        public IllusionaryClone(Mobile master)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(100);
            SetDex(100);
            SetInt(100);

            SetHits(100);

            SetDamage(5, 10);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 50);

            VirtualArmor = 50;

            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(Delete));
        }

        public IllusionaryClone(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (m_Master == null || m_Master.Deleted || !m_Master.Alive)
            {
                Delete();
                return;
            }

            if (Combatant == null || Combatant.Deleted || !Combatant.Alive)
            {
                Attack(m_Master);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
