using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Spells.Mysticism;
using Server.Spells.Necromancy;

namespace Server.Mobiles
{
    [CorpseName("an abyssal husk")]
    public class AbyssalManefall : BaseCreature
    {
        private DateTime m_NextMindBreak;
        private DateTime m_NextRealityBreach;
        private DateTime m_NextManaDrainNova;

        [Constructable]
        public AbyssalManefall()
            : base(AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Abyssal Manefall";
            Body = 740;               // Same as VoidManifestation
            Hue = 2657;               // Deep purple-blue iridescence
            BaseSoundID = 684;

            SetStr(700);
            SetDex(180);
            SetInt(800);

            SetHits(4200);
            SetMana(120000);
            SetStam(300);

            SetDamage(28, 42);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Cold, 40);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 65, 80);

            SetSkill(SkillName.Mysticism, 120);
            SetSkill(SkillName.Focus, 120);
            SetSkill(SkillName.Magery, 130.0);
            SetSkill(SkillName.EvalInt, 130.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Necromancy, 120.0);
            SetSkill(SkillName.SpiritSpeak, 120.0);
            SetSkill(SkillName.Wrestling, 135.0);
            SetSkill(SkillName.MagicResist, 160.0);
            SetSkill(SkillName.Tactics, 140.0);

            Fame = 32000;
            Karma = -32000;
            VirtualArmor = 90;
        }

        public override bool AlwaysMurderer => true;
        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextMindBreak)
                    CastMindBreak();

                if (DateTime.UtcNow >= m_NextRealityBreach)
                    TriggerRealityBreach();

                if (DateTime.UtcNow >= m_NextManaDrainNova)
                    ManaDrainNova();
            }
        }

        private void CastMindBreak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x9C, true, "*Abyssal Manefall unleashes a soul-shattering shriek!*");
            PlaySound(0x54A);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m == this || !m.Alive)
                    continue;

                if (m is Mobile target)
                {
                    target.SendMessage(0x22, "Your mind feels torn by dimensional echoes!");
                    AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);

                    target.FixedParticles(0x3728, 1, 18, 9910, 92, 3, EffectLayer.Head);

                    if (Utility.RandomDouble() < 0.2)
                        target.Paralyze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextMindBreak = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void TriggerRealityBreach()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*Reality distorts around the Manefall...*");

            Map map = Map;

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(
                    Location.X + Utility.RandomMinMax(-2, 2),
                    Location.Y + Utility.RandomMinMax(-2, 2),
                    Location.Z);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), () =>
                {
                    Effects.SendLocationEffect(loc, map, 0x3709, 16, 1);
                    PlaySound(0x65A);
                    foreach (Mobile m in GetMobilesInRange(2))
                    {
                        if (m is Mobile target && m != this && target.Alive)
                        {
                            target.SendMessage("The air rends around you!");
                            AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 50, 0, 50, 0);
                        }
                    }
                });
            }

            m_NextRealityBreach = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void ManaDrainNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x9C, true, "*A wave of voidal hunger erupts outward!*");

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m is Mobile target && target.Alive && target.Mana > 0)
                {
                    int drained = Utility.RandomMinMax(20, 40);
                    if (target.Mana < drained) drained = target.Mana;

                    target.Mana -= drained;
                    this.Mana += drained;

                    target.SendMessage(0x22, "You feel your magical energy being siphoned!");
                    target.FixedParticles(0x374A, 10, 30, 5013, 92, 2, EffectLayer.CenterFeet);
                }
            }

            m_NextManaDrainNova = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.HighScrolls, 4);
            AddLoot(LootPack.Gems, 6);
        }

        public AbyssalManefall(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextMindBreak = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextRealityBreach = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            m_NextManaDrainNova = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }
    }
}
