using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a plaguebound feline corpse")]
    public class PlagueboundFeline : BaseCreature
    {
        private DateTime m_NextPlaguePounce;
        private DateTime m_NextDreamCurse;
        private DateTime m_NextPhaseShift;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public PlagueboundFeline()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a plaguebound feline";
            Body = 0xC9;
            BaseSoundID = 0x69;
            Hue = 2956; // Sickly violet-green hue

            SetStr(200, 250);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(700, 850);
            SetMana(500, 700);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 95.0);
            SetSkill(SkillName.Poisoning, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;
        }

        public override bool Unprovokable => true;
        public override bool AlwaysMurderer => true;
        public override bool BleedImmune => true;
        public override Poison HitPoison => Poison.Deadly;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new PlaguebearersCloak());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Combatant.Deleted || !Combatant.Alive)
                return;

            if (!m_AbilitiesInitialized)
            {
                m_NextPlaguePounce = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                m_NextDreamCurse = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextPlaguePounce)
                PlaguePounce();

            if (DateTime.UtcNow >= m_NextDreamCurse)
                DreamCurse();

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();
        }

        private void PlaguePounce()
        {
            if (Combatant is Mobile target && InRange(target, 10))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The feline vanishes in a blur and strikes!*");

                Point3D targetLoc = target.Location;
                MoveToWorld(targetLoc, Map);

                Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x374A, 10, 15, 2956, 0, 5022, 0);

                AOS.Damage(target, this, Utility.RandomMinMax(25, 35), 0, 0, 0, 50, 50);
                target.SendMessage("You are raked by diseased claws!");
                target.ApplyPoison(this, Poison.Deadly);
            }

            m_NextPlaguePounce = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void DreamCurse()
        {
            if (Combatant is Mobile m && Utility.RandomDouble() < 0.8)
            {
                PublicOverheadMessage(MessageType.Emote, 0x22, true, "*Its eyes shimmer with haunting light...*");

                m.FixedParticles(0x373A, 10, 15, 5018, 2956, 0, EffectLayer.Head);
                m.SendMessage(0x22, "You feel your dreams darken...");
                m.SendLocalizedMessage(1042523); // Optional flavor message
                m.Paralyze(TimeSpan.FromSeconds(2 + Utility.RandomDouble() * 2));
                AOS.Damage(m, this, Utility.RandomMinMax(15, 20), 0, 100, 0, 0, 0);
            }

            m_NextDreamCurse = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, true, "*The feline fades into a cloud of spores...*");
            Effects.PlaySound(Location, Map, 0x229);
            Hidden = true;

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                Hidden = false;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*It re-emerges with a spectral hiss!*");
                Effects.SendLocationEffect(Location, Map, 0x37C4, 10);
            });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public PlagueboundFeline(Serial serial) : base(serial)
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
            m_AbilitiesInitialized = false;
        }
    }
}
