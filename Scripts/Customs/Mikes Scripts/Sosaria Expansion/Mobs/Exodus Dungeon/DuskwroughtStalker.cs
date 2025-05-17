using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;


namespace Server.Mobiles
{
    [CorpseName("a duskwrought stalker corpse")]
    public class DuskwroughtStalker : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextShroudWave;
        private DateTime m_NextVoidGaze;

        [Constructable]
        public DuskwroughtStalker()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Duskwrought Stalker";
            Body = 313; // Darknight Creeper body
            Hue = 1175; // Ethereal violet-black glow
            BaseSoundID = 0xE0;

            SetStr(450, 500);
            SetDex(120, 150);
            SetInt(500, 600);

            SetHits(7500);
            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 65);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Poisoning, 150.0);
            SetSkill(SkillName.DetectHidden, 120.0);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 50;

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
            m_NextShroudWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 45));
            m_NextVoidGaze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(60, 120));
        }

        public DuskwroughtStalker(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override bool Unprovokable => true;
        public override bool BleedImmune => true;
        public override bool BardImmune => true;
        public override bool CanFlee => false;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.UltraRich);

            if (Utility.RandomDouble() < 0.005) // Rare drop
                PackItem(new UmbralVeilFragment());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextPhaseShift)
                    PhaseShift();

                if (DateTime.UtcNow >= m_NextShroudWave)
                    ShroudWave();

                if (DateTime.UtcNow >= m_NextVoidGaze)
                    VoidGaze();
            }
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*flickers from view and reappears nearby in a swirl of shadows*");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x37CC, 1, 15, Hue, 0, 5022, 0);
            PlaySound(0x20E); // Teleport sound

            Point3D newLocation = new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z);
            MoveToWorld(newLocation, Map);

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 45));
        }

        private void ShroudWave()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, false, "*unleashes a wave of shadow from its core*");
            PlaySound(0x64C);

            Effects.PlaySound(Location, Map, 0x64C);
            Effects.SendLocationEffect(Location, Map, 0x3709, 30);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m.Player)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 0, 100); // Pure energy
                    m.FixedParticles(0x374A, 10, 25, 5021, 1175, 0, EffectLayer.Waist);
                    m.SendMessage(38, "A chilling shadow pierces through your soul!");
                }
            }

            m_NextShroudWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(40, 60));
        }

        private void VoidGaze()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Emote, 0x22, false, "*locks eyes with its foe, channeling voidal terror*");
                PlaySound(0x56E);

                Mobile target = Combatant as Mobile;

                target.FixedParticles(0x37BE, 10, 25, 5032, 1175, 0, EffectLayer.Head);
                target.SendMessage(38, "You feel your mind slipping into oblivion...");
                target.Paralyze(TimeSpan.FromSeconds(4));
                AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 0, 100, 0, 0); // Energy damage

                m_NextVoidGaze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(90, 120));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class UmbralVeilFragment : Item
    {
        [Constructable]
        public UmbralVeilFragment() : base(0x2C3A)
        {
            Name = "Umbral Veil Fragment";
            Hue = 1175;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public UmbralVeilFragment(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
