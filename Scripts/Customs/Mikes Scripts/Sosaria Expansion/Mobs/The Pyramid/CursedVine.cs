using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cursed vine corpse")]
    public class CursedVine : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextThornBarrage;
        private DateTime m_NextToxicSpore;
        private DateTime m_NextEntangle;
        private DateTime m_NextSummon;

        // Unique Jade Hue
        private const int UniqueHue = 0x48E;

        [Constructable]
        public CursedVine()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Cursed Vine";
            Body = 8;
            BaseSoundID = 352;
            Hue = UniqueHue;

            // Stats
            SetStr(400, 480);
            SetDex(180, 220);
            SetInt(150, 200);

            SetHits(2000, 2400);
            SetStam(180, 220);
            SetMana(300, 400);

            SetDamage(20, 35);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Tactics, 95.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initial cooldowns
            m_NextThornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextToxicSpore    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextEntangle      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummon        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Basic loot
            PackItem(new FertileDirt(Utility.RandomMinMax(3, 7)));
            PackItem(new Vines());
            PackReg(5);
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus    => 80.0;

        public CursedVine(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            if (DateTime.UtcNow >= m_NextThornBarrage && InRange(Combatant.Location, 12))
            {
                ThornBarrage();
                m_NextThornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            else if (DateTime.UtcNow >= m_NextToxicSpore)
            {
                ToxicSporeCloud();
                m_NextToxicSpore = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            else if (DateTime.UtcNow >= m_NextEntangle && InRange(Combatant.Location, 14))
            {
                EntanglingRoots();
                m_NextEntangle = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            else if (DateTime.UtcNow >= m_NextSummon)
            {
                SummonVineSpawns();
                m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.20 >= Utility.RandomDouble() && defender is Mobile m)
            {
                DoHarmful(m);
                m.SendMessage("Thorny vines lash into you!");
                m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                m.PlaySound(0x56E);

                // Apply bleeding — use the 3‐arg overload only
                AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);

                // Slow for a moment
                m.Freeze(TimeSpan.FromSeconds(2.0));
            }
        }

        private void ThornBarrage()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*The vines spit their thorns!*");
            PlaySound(0x56C);

            var projectiles = 5;
            var range = 10;
            var sent = new List<Mobile> { target };

            for (int i = 0; i < projectiles; i++)
            {
                Mobile last = sent[sent.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !sent.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }

                if (next != null)
                    sent.Add(next);
                else
                    break;
            }

            for (int i = 0; i < sent.Count; i++)
            {
                Mobile src = (i == 0 ? this : sent[i - 1]);
                Mobile dst = sent[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x26BD, 7, 0, false, false, UniqueHue, 0, 9501, 0, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);
                        dst.PlaySound(0x56D);
                    }
                });
            }
        }

        private void ToxicSporeCloud()
        {
            Say("*Miasma blooms!*");
            PlaySound(0x57C);

            for (int i = 0; i < 4; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                Point3D loc = new Point3D(x, y, z);
                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                var tile = new PoisonTile { Hue = UniqueHue };
                tile.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x375A, 10, 15, UniqueHue, 0, 5033, 0);
            }
        }

        private void EntanglingRoots()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Roots erupt!*");
            PlaySound(0x56F);

            // Root effect
            target.Freeze(TimeSpan.FromSeconds(3.0));
            target.SendMessage("Thick vines entangle your legs!");

            // Use CenterFeet, not Feet
            target.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            // Place a web trap under them
            var web = new TrapWeb { Hue = UniqueHue };
            web.MoveToWorld(target.Location, Map);
        }

        private void SummonVineSpawns()
        {
            Say("*Grow… sprout… serve!*");
            PlaySound(0x57E);

            int count = 2;
            for (int i = 0; i < count; i++)
            {
                Point3D spawnLoc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z);

                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);

                var vine = new WhippingVine();
                vine.Hue = UniqueHue;
                vine.MoveToWorld(spawnLoc, Map);
                vine.Combatant = Combatant;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            PlaySound(0x58B);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 60, UniqueHue, 0, 5052, 0);

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;

                Point3D loc = new Point3D(x, y, z);
                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                var web = new TrapWeb { Hue = UniqueHue };
                web.MoveToWorld(loc, Map);
            }
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
        }
    }
}
