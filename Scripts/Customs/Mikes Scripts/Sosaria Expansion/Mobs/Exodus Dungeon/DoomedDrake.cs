using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a doomed drake corpse")]
    public class DoomedDrake : BaseCreature
    {
        private DateTime m_NextVoidPulse;
        private DateTime m_NextRealityTear;
        private bool m_FateReversalReady;

        [Constructable]
        public DoomedDrake() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Doomed Drake";
            Body = 0x58E;
            Hue = 2101;
            BaseSoundID = 362;


            SetStr(900, 1000);
            SetDex(100, 130);
            SetInt(600, 750);

            SetHits(1200, 1400);
            SetDamage(22, 28);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Fire, 30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 65, 75);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);

            Fame = 26000;
            Karma = -26000;
            VirtualArmor = 65;

            Tamable = false;

            m_NextVoidPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextRealityTear = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
            m_FateReversalReady = true;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextVoidPulse)
                    VoidPulse();

                if (DateTime.UtcNow >= m_NextRealityTear)
                    RealityTear();

                if (Utility.RandomDouble() < 0.01 && m_FateReversalReady)
                    FateReversal();
            }
        }

        private void VoidPulse()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*The Doomed Drake pulses with unstable void energy!*");
            PlaySound(0x5C5);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(25, 40);
                    AOS.Damage(m, this, damage, 0, 25, 25, 25, 25);

                    if (Utility.RandomDouble() < 0.3 && m is Mobile target)
                    {
                        target.FixedParticles(0x37BE, 10, 30, 5012, EffectLayer.Head);
                        target.SendMessage(0x22, "Your soul trembles under the weight of void energy!");
                        target.Paralyze(TimeSpan.FromSeconds(2 + Utility.RandomDouble() * 3));
                    }
                }
            }

            m_NextVoidPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }

        private void RealityTear()
        {
            PublicOverheadMessage(MessageType.Emote, 0x23, true, "*Reality fractures around the Doomed Drake!*");
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10);

            Point3D newLoc = new Point3D(
                Location.X + Utility.RandomMinMax(-5, 5),
                Location.Y + Utility.RandomMinMax(-5, 5),
                Location.Z
            );

            Map map = Map;
            if (map != null)
                MoveToWorld(newLoc, map);

            m_NextRealityTear = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
        }

        private void FateReversal()
        {
            m_FateReversalReady = false;
            PublicOverheadMessage(MessageType.Emote, 0x22, true, "*The Doomed Drake shimmers with reversed fate!*");

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                m_FateReversalReady = true;
                PublicOverheadMessage(MessageType.Emote, 0x22, true, "*The Fate Reversal fades.*");
            });
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (m_FateReversalReady && caster != null && !Deleted)
            {
                caster.SendMessage(0x22, "Your spell rebounds upon you!");
                AOS.Damage(caster, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 0, 100);
                PlaySound(0x5CF);
                m_FateReversalReady = false;
            }

            base.OnDamagedBySpell(caster);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2 && attacker is Mobile target)
            {
                target.SendMessage(0x22, "You feel a withering aura sapping your vitality!");
                target.AddStatMod(new StatMod(StatType.Str, "DoomAuraStr", -5, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Int, "DoomAuraInt", -5, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Dex, "DoomAuraDex", -5, TimeSpan.FromSeconds(30)));
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, 4);
        }

        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;
        public override int TreasureMapLevel => 4;
        public override int Meat => 12;
        public override int DragonBlood => 10;
        public override int Hides => 30;
        public override HideType HideType => HideType.Barbed;
        public override int Scales => 4;
        public override ScaleType ScaleType => ScaleType.Black;
        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
        public override bool CanFly => true;

        public DoomedDrake(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_FateReversalReady = true;
        }
    }
}
