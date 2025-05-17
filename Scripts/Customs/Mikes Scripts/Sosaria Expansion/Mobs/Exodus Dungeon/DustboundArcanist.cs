using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the dust of an arcanist")]
    public class DustboundArcanist : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextHexBurst;
        private DateTime m_NextAstralChains;
        private DateTime m_NextWailOfDust;
        private bool m_Initialized;

        [Constructable]
        public DustboundArcanist()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Dustbound Arcanist";
            Body = 765; // Juka Mage body
            Hue = 1150; // Pale ashen-grey hue
            BaseSoundID = 0x1AC;

            SetStr(300, 400);
            SetDex(100, 120);
            SetInt(600, 750);

            SetHits(900, 1200);
            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 40);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 130.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 65;

            m_Initialized = false;
        }

        public override int TreasureMapLevel => 4;
        public override bool CanRummageCorpses => true;
        public override bool AlwaysMurderer => true;
        public override bool AutoDispel => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_Initialized)
            {
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                m_NextHexBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
                m_NextAstralChains = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
                m_NextWailOfDust = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 70));
                m_Initialized = true;
            }

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();

            if (DateTime.UtcNow >= m_NextHexBurst)
                HexBurst();

            if (DateTime.UtcNow >= m_NextAstralChains)
                CastAstralChains();

            if (DateTime.UtcNow >= m_NextWailOfDust)
                WailOfDust();
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*shimmers and vanishes into the veil of dust*");
            Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(6.0), () =>
            {
                Hidden = false;
                PublicOverheadMessage(MessageType.Emote, 0x480, false, "*emerges from the dust with eyes aglow*");
            });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 50));
        }

        private void HexBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Dustbound Arcanist releases a hex burst!*");
            PlaySound(0x1D0); // Harmful sound

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.FixedParticles(0x37C4, 10, 25, 5030, EffectLayer.Head);
                    m.PlaySound(0x1E1);

                    if (m is Mobile target)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 30, 0, 0, 70);
                        target.SendMessage(0x22, "Your mind burns with arcane backlash!");
                    }
                }
            }

            m_NextHexBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
        }

        private void CastAstralChains()
        {
            if (Combatant is Mobile target && target.Alive && !target.Hidden)
            {
                PublicOverheadMessage(MessageType.Emote, 0x480, false, "*chains of dust and light bind their foe*");
                target.FixedParticles(0x376A, 9, 32, 5032, EffectLayer.Waist);
                target.PlaySound(0x1EC);

                target.Freeze(TimeSpan.FromSeconds(4.0));
                target.SendMessage(0x22, "You are bound by ethereal chains and cannot move!");

                m_NextAstralChains = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
            }
        }

        private void WailOfDust()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*lets out a haunting, dust-filled wail*");
            PlaySound(0x28D); // Death scream

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(25, 40);
                    AOS.Damage(m, this, damage, 10, 10, 10, 10, 60); // Primarily Energy

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage(0x22, "You are wracked by a supernatural scream!");
                        mobile.Paralyze(TimeSpan.FromSeconds(2.5));
                    }
                }
            }

            m_NextWailOfDust = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
        }

        public override int GetIdleSound() => 0x1AC;
        public override int GetAngerSound() => 0x1CD;
        public override int GetHurtSound() => 0x1D0;
        public override int GetDeathSound() => 0x28D;

        public DustboundArcanist(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Initialized = false;
        }
    }
}
