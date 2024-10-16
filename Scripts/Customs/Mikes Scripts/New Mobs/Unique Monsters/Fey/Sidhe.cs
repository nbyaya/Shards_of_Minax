using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a sidhe corpse")]
    public class Sidhe : BaseCreature
    {
        private DateTime m_NextRoyalAura;
        private DateTime m_NextTimeStop;
        private DateTime m_NextMirrorImage;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Sidhe()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a sidhe";
            Body = 128; // GreenGoblin body
            BaseSoundID = 0x4B0; // Pixie sound
            Hue = 1580; // Ethereal blue hue

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

            PackItem(new PotionKeg());

            m_AbilitiesInitialized = false; // Set the initialization flag to false
        }

        public Sidhe(Serial serial)
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
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextRoyalAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTimeStop = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRoyalAura)
                {
                    RoyalAura();
                }

                if (DateTime.UtcNow >= m_NextTimeStop)
                {
                    TimeStop();
                }

                if (DateTime.UtcNow >= m_NextMirrorImage)
                {
                    MirrorImage();
                }
            }
        }

        private void RoyalAura()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The Sidhe's royal aura enhances nearby fairies *");

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m is BaseCreature && ((BaseCreature)m).Tribe == TribeType.Fey)
                {
                    m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    m.PlaySound(0x1EA);
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.PsychicAttack, 1075655, 1075656, TimeSpan.FromSeconds(60.0), m, "10\t10\t10\t10"));
                }
            }

            m_NextRoyalAura = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for RoyalAura
        }

        private void TimeStop()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Time slows to a crawl around the Sidhe *");

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && !(m is Sidhe))
                {
                    m.Frozen = true;
                    m.Paralyzed = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(delegate
                    {
                        m.Frozen = false;
                        m.Paralyzed = false;
                        m.SendLocalizedMessage(1005603); // You can move again!
                    }));
                }
            }

            m_NextTimeStop = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for TimeStop
        }

        private void MirrorImage()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The Sidhe splits into multiple images *");

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    SidheMirrorImage image = new SidheMirrorImage(this);
                    image.MoveToWorld(loc, Map);

                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate
                    {
                        if (!image.Deleted)
                            image.Delete();
                    }));
                }
            }

            m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for MirrorImage
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

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }

    public class SidheMirrorImage : BaseCreature
    {
        private Mobile m_Master;

        public SidheMirrorImage(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);

            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public SidheMirrorImage(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
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
