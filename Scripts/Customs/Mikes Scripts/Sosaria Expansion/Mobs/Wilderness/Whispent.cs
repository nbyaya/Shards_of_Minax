using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;           // SpellHelper, etc.
using Server.Spells.Sixth;     // ParalyzeSpell
using Server.Spells.Seventh;   // PolymorphSpell

namespace Server.Mobiles
{
    [CorpseName("a wispy corpse")]
    public class Whispent : BaseCreature
    {
        private DateTime _NextAbilityUse;
        private DateTime _NextPhaseShift;

        [Constructable]
        public Whispent()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Whispent";
            Body = 0x3CA;
            BaseSoundID = 0x107;
            Hue = 0x400C;

            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(400, 500);

            SetHits(1500, 2000);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics,     90.0, 100.0);
            SetSkill(SkillName.Wrestling,   80.0,  90.0);
            SetSkill(SkillName.Focus,      100.0, 100.0);      // was CastingFocus :contentReference[oaicite:5]{index=5}
            SetSkill(SkillName.SpiritSpeak,100.0, 110.0);      // was Spiritualism :contentReference[oaicite:6]{index=6}

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 40;

            PackGold(500, 1000);

            _NextAbilityUse  = DateTime.UtcNow;
            _NextPhaseShift  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
        }

        public Whispent(Serial serial)
            : base(serial)
        {
        }

        public override int GetIdleSound()  { return 0x107; }
        public override int GetAngerSound() { return 0x1BF; }
        public override int GetDeathSound() { return 0xFD; }

        public override bool AlwaysAttackable { get { return true; } }
        public override bool BleedImmune       { get { return true; } }
        public override int  TreasureMapLevel  { get { return 4; } }

        public override void DisplayPaperdollTo(Mobile to) { }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);      // was VeryRich :contentReference[oaicite:7]{index=7}
            PackReg(5);                       // drops 5 reagents instead of MageRegs :contentReference[oaicite:8]{index=8}
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            Mobile target = Combatant as Mobile;

            if (target != null && !target.Deleted && target.Map == Map && InRange(target,15) && CanBeHarmful(target) && InLOS(target))
            {
                if (DateTime.UtcNow >= _NextAbilityUse)
                {
                    SpectralGale(target);
                    _NextAbilityUse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }

                if (DateTime.UtcNow >= _NextPhaseShift)
                {
                    PhaseShift();
                    _NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                }

                if (Utility.RandomDouble() < 0.15) // Spirit Drain chance
                {
                    SpiritDrain(target);
                }
            }
        }

        public void SpectralGale(Mobile target)
        {
            Say("*The Whispent gathers spectral winds!*");
            // simple cast animation (32 is a generic “cast” action)
            Animate(32, 5, 1, true, false, 0);

            // wind‐up particles at feet
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            Point3D loc = target.Location;

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (Deleted || !Alive) return;

                // burst at target location
                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, 5052
                ); // :contentReference[oaicite:9]{index=9}

                Effects.PlaySound(loc, Map, 0x10F);

                int radius = 5;
                List<Mobile> list = new List<Mobile>();
                foreach (Mobile m in Map.GetMobilesInRange(loc, radius))
                {
                    if (m != this && CanBeHarmful(m))
                    {
                        list.Add(m);
                        DoHarmful(m);
                    }
                }

                foreach (Mobile m in list)
                {
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, dmg, 0, 0, 50, 0, 50);

                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(1, 4)));
                        m.SendLocalizedMessage(502399); // You are frozen solid!
                    }
                }
            });
        }

        public void PhaseShift()
        {
            Point3D oldLoc = Location;
            Map m = Map;
            if (m == null) return;

            Point3D newLoc = oldLoc;
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-5, 5);
                int y = Y + Utility.RandomMinMax(-5, 5);
                int z = m.GetAverageZ(x, y);
                if (m.CanSpawnMobile(x,y,z) && m.CanFit(x,y,z,16,false,false))
                {
                    newLoc = new Point3D(x, y, z);
                    break;
                }
            }

            if (newLoc != oldLoc)
            {
                Say("*The Whispent phases out!*");
                Effects.SendLocationParticles(
                    EffectItem.Create(oldLoc,  m, EffectItem.DefaultDuration),
                    0x3728, 10, 10, 2023
                );
                Effects.PlaySound(oldLoc, m, 0x1FE);

                MoveToWorld(newLoc, m);

                Effects.SendLocationParticles(
                    EffectItem.Create(newLoc, m, EffectItem.DefaultDuration),
                    0x3728, 10, 10, 2023
                );
                Effects.PlaySound(newLoc, m, 0x1FE);
            }
        }

        public void SpiritDrain(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive || !CanBeHarmful(target)) return;

            DoHarmful(target);
            Animate(32, 5, 1, true, false, 0);

            PlaySound(0x1E4);

            int mDrain = Utility.RandomMinMax(20, 40);
            int sDrain = Utility.RandomMinMax(20, 40);

            target.Mana = Math.Max(0, target.Mana - mDrain);
            target.Stam = Math.Max(0, target.Stam - sDrain);

            target.SendLocalizedMessage(1060084);
            target.SendLocalizedMessage(1060085);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (Utility.RandomDouble() < 0.1 && DateTime.UtcNow >= _NextPhaseShift)
            {
                PhaseShift();
                _NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
            }
            base.OnGotMeleeAttack(attacker);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _NextAbilityUse = DateTime.UtcNow;
            _NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
        }
    }
}
