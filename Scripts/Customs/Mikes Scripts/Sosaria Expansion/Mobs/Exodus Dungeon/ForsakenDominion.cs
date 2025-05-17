using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a forsaken dominion's husk")]
    public class ForsakenDominion : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextRealityTear;
        private DateTime m_NextRoyalDecree;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ForsakenDominion()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "the Forsaken Dominion";
            Body = 0x310; // Arcane daemon body
            Hue = 1175; // Dark cosmic purple
            BaseSoundID = 0x47D;

            SetStr(900, 1000);
            SetDex(200, 250);
            SetInt(800, 1000);

            SetHits(1500, 1800);

            SetDamage(28, 38);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 110.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 28000;
            Karma = -28000;

            VirtualArmor = 80;

            m_AbilitiesInitialized = false;
        }

        public override Poison PoisonImmune => Poison.Lethal;

        public override bool AutoDispel => true;
        public override bool CanRummageCorpses => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.03)
            {
                PackItem(new CrownOfDominion());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_AbilitiesInitialized)
            {
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                m_NextRealityTear = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
                m_NextRoyalDecree = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();

            if (DateTime.UtcNow >= m_NextRealityTear)
                RealityTear();

            if (DateTime.UtcNow >= m_NextRoyalDecree)
                RoyalDecree();
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Emote, 0x482, true, "*The Forsaken Dominion phases between dimensions!*");
            this.Hidden = true;

            Timer.DelayCall(TimeSpan.FromSeconds(4), () =>
            {
                this.Hidden = false;
                this.PlaySound(0x56B); // magical reappearance
                this.Hits += 100; // Heals a bit when reappearing
                Effects.SendLocationEffect(this.Location, this.Map, 0x3728, 12, 1, Hue, 0);
            });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 45));
        }

        private void RealityTear()
        {
            PublicOverheadMessage(MessageType.Emote, 0x23B, true, "*Reality tears open around the Dominion!*");
            Effects.PlaySound(Location, Map, 0x10B); // Arcane burst

            foreach (Mobile target in GetMobilesInRange(5))
            {
                if (target != this && target.Alive && !target.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(35, 60);
                    AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
                    target.SendMessage(0x22, "You are struck by the raw power of a dimensional rift!");
                    target.Freeze(TimeSpan.FromSeconds(2));
                    Effects.SendBoltEffect(target);
                }
            }

            m_NextRealityTear = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void RoyalDecree()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*The Forsaken Dominion invokes a Royal Decree from a forgotten age!*");

            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                target.SendMessage(0x22, "A terrible force binds your soul!");
                target.Freeze(TimeSpan.FromSeconds(5));
                Effects.SendTargetParticles(target, 0x36BD, 10, 15, 2023, EffectLayer.Head);
                target.PlaySound(0x1FB);

                Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                {
                    if (target != null && target.Alive)
                    {
                        int soulBurn = Utility.RandomMinMax(40, 70);
                        AOS.Damage(target, this, soulBurn, 0, 0, 0, 0, 100);
                        target.SendMessage(0x22, "You feel your essence unravel!");
                    }
                });
            }

            m_NextRoyalDecree = DateTime.UtcNow + TimeSpan.FromSeconds(90);
        }

        public ForsakenDominion(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class CrownOfDominion : BaseHat
    {
        [Constructable]
        public CrownOfDominion() : base(0x2FB9) // Unique crown model
        {
            Name = "Crown of the Forsaken Dominion";
            Hue = 1175;
            Attributes.CastRecovery = 3;
            Attributes.SpellDamage = 20;
            Attributes.BonusInt = 5;
            Resistances.Energy = 10;
        }

        public CrownOfDominion(Serial serial) : base(serial) { }

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
