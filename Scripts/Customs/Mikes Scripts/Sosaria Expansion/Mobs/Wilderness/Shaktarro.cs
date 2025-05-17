using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Sixth;
using Server.Spells.Seventh;

namespace Server.Mobiles
{
    [CorpseName("a shaktarro corpse")]
    public class Shaktarro : BaseCreature
    {
        private DateTime _NextWindsweep;
        private DateTime _NextShadowStep;

        public override int Hue => 0x48F;

        [Constructable]
        public Shaktarro()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Shaktarro";
            Title = "of the Blighted Wind";
            Body = 400;
            BaseSoundID = 0x47D;

            SetStr(850, 950);
            SetDex(350, 450);
            SetInt(450, 550);

            SetHits(2800, 3500);
            SetDamage(25, 35);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Tactics,      100.0, 110.0);
            SetSkill(SkillName.Wrestling,    100.0, 110.0);
            SetSkill(SkillName.Swords,       100.0, 110.0);
            SetSkill(SkillName.Anatomy,      100.0, 110.0);
            SetSkill(SkillName.EvalInt,       90.0, 100.0);
            SetSkill(SkillName.Mysticism,     90.0, 100.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 75;

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public Shaktarro(Serial serial) : base(serial) { }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.20 > Utility.RandomDouble())
            {
                Poison p = (Utility.RandomBool() ? Poison.Deadly : Poison.Greater);
                defender.ApplyPoison(this, p);
                defender.PlaySound(0x205);
                defender.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
            }
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= _NextWindsweep && InRange(target, 10))
                {
                    _NextWindsweep = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                    PerformWindsweep(target);
                }

                if (DateTime.UtcNow >= _NextShadowStep && !InRange(target, 3))
                {
                    _NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    PerformShadowStep(target);
                }

                if (Utility.RandomDouble() < 0.1 && Mana > 50)
                {
                    PerformMindRend(target);
                }
            }
        }

		public static void PushBack(Mobile from, Mobile to, int distance)
		{
			if (from == null || to == null || from.Map != to.Map)
				return;

			int dx = to.X - from.X;
			int dy = to.Y - from.Y;

			int pushX = to.X + (dx != 0 ? (dx / Math.Abs(dx)) * distance : 0);
			int pushY = to.Y + (dy != 0 ? (dy / Math.Abs(dy)) * distance : 0);
			int pushZ = to.Map.GetAverageZ(pushX, pushY);

			Point3D newLoc = new Point3D(pushX, pushY, pushZ);
			if (to.Map.CanSpawnMobile(newLoc))
			{
				to.MoveToWorld(newLoc, to.Map);
				to.SendMessage("You are pushed back by the blighted wind!");
			}
		}


        public void PerformWindsweep(Mobile target)
        {
            MovingParticles(target, 0x3709, 10, 30, false, true, 0x36E, 0xF00, 0x1ED0, 0, 0, EffectLayer.Waist, 0);
            PlaySound(0x665);

            Timer.DelayCall(TimeSpan.FromSeconds(2.5), () =>
            {
                PlaySound(0x23E);
                FixedEffect(0x3709, 25, 25, 0x3EE, 0);

                var mobilesInRange = GetMobilesInRange(5);
                foreach (Mobile m in mobilesInRange)
                {
                    // skip self, untargetable, or dead bonded pets
                    if (m == this || !CanBeHarmful(m) ||
                        (m is BaseCreature bc && bc.IsBonded && bc.IsDeadPet))
                        continue;

                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(m, this, damage, 50, 0, 0, 0, 50);

                    ApplySlowDebuff(m, TimeSpan.FromSeconds(8), 15);

                    if (m is Mobile mv)
                    {
                        mv.Stam = Math.Max(0, mv.Stam - Utility.RandomMinMax(10, 20));
                        mv.SendMessage("The blighted wind saps your energy!");
                    }

                    PushBack(this, m, 1);
                }
                mobilesInRange.Free();
            });
        }

		public void ApplySlowDebuff( Mobile m, TimeSpan duration, int percentReduction )
		{
/* 			if ( !(m is PlayerMobile || m is BaseCreature) )
				return;

			// we're going to subtract percentReduction from their physical resist
			var debuffMod = new ResistanceMod( ResistanceType.Physical, -percentReduction );

			// see if they already have that exact debuff
			bool already = m.ResistanceMods
							.OfType<ResistanceMod>()
							.Any( rm => rm.Type   == ResistanceType.Physical
									 && rm.Offset == -percentReduction );

			if ( already )
				return;

			// apply and schedule removal
			m.AddResistanceMod( debuffMod );
			m.SendMessage( "You are slowed by the blighted wind!" );

			Timer.DelayCall( duration, () =>
			{
				m.RemoveResistanceMod( debuffMod );
				m.SendMessage( "The blighted wind's effect fades." );
			} ); */
		}



        public void PerformShadowStep(Mobile target)
        {
            if (target == null || target.Deleted || target.Map == Map.Internal || !target.Alive)
                return;

            PlaySound(0x51B);
            FixedParticles(0x3709, 10, 30, 9504, EffectLayer.CenterFeet);

            var map = Map;
            Point3D dest = GetRandomLocation(target, 5);
            if (map.CanSpawnMobile(dest))
            {
                MoveToWorld(dest, map);
                PlaySound(0x51B);
                FixedParticles(0x3709, 10, 30, 9504, EffectLayer.CenterFeet);
                SendMessage("Shaktarro vanishes and reappears!");
                Combatant = target;
            }
            else
            {
                // small penalty for failing
                _NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(2.0);
            }
        }

        public Point3D GetRandomLocation(Mobile target, int range)
        {
            var p = target.Location;
            var map = Map;

            for (int i = 0; i < 10; i++)
            {
                int x = p.X + Utility.RandomMinMax(-range, range);
                int y = p.Y + Utility.RandomMinMax(-range, range);
                int z = map.GetAverageZ(x, y);

                var test = new Point3D(x, y, z);
                if (map.CanSpawnMobile(test))
                    return test;
            }
            return Location;
        }

        public void PerformMindRend(Mobile t)
        {
            if (!(t is Mobile targetMobile) || targetMobile.Deleted || targetMobile.Map == Map.Internal || !targetMobile.Alive)
                return;

            DoHarmful(targetMobile);
            targetMobile.PlaySound(0x1F1);
            targetMobile.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Head);

            double damage = Utility.RandomMinMax(30, 40);
            double evalInt = Skills[SkillName.EvalInt].Value;
            double resist  = targetMobile.Skills[SkillName.MagicResist].Value;
            damage *= (evalInt / Math.Max(1.0, resist * 0.75));

            AOS.Damage(targetMobile, this, (int)damage, 0, 0, 0, 0, 100);

            int manaDrain = Utility.RandomMinMax(20, 30);
            int stamDrain = Utility.RandomMinMax(20, 30);
            targetMobile.Mana = Math.Max(0, targetMobile.Mana - manaDrain);
            targetMobile.Stam = Math.Max(0, targetMobile.Stam - stamDrain);
            targetMobile.SendAsciiMessage("Your mind is assaulted, draining your energy!");
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 8);
            AddLoot(LootPack.HighScrolls, 4);
            AddLoot(LootPack.MedScrolls, 4);
            AddLoot(LootPack.Potions, 6);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.02)
                PackItem(new BladeOfBlight());

            if (Utility.RandomDouble() < 0.10)
                PackItem(new BlightedEssence());
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new TigerClawKey());
            if (Utility.RandomDouble() < 0.5)
                c.DropItem(new TigerClawSectBadge());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
            writer.Write(_NextWindsweep);
            writer.Write(_NextShadowStep);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
            {
                _NextWindsweep  = reader.ReadDateTime();
                _NextShadowStep = reader.ReadDateTime();
            }
        }
    }

    // -------------------------------------------------------------------------
    // Stubs for the two custom loot items so your script will compile:
    // -------------------------------------------------------------------------

    public class BladeOfBlight : Item
    {
        [Constructable]
        public BladeOfBlight() : base(0x13B2)
        {
            Name = "Blade of Blight";
            Hue  = 0x48F;
            Weight = 1.0;
        }

        public BladeOfBlight(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlightedEssence : Item
    {
        [Constructable]
        public BlightedEssence() : base(0x1AEA)
        {
            Name = "Blighted Essence";
            Hue  = 0x48F;
            Weight = 0.5;
        }

        public BlightedEssence(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
