using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a glimmerjack corpse")]
    public class Glimmerjack : BaseCreature
    {
        private DateTime _NextNova;
        private DateTime _NextTeleport;

        [Constructable]
        public Glimmerjack()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Glimmerjack";
            Body = 74;
            BaseSoundID = 422;
            Hue = 0x490;

            SetStr(250, 300);
            SetDex(150, 200);
            SetInt(300, 350);

            SetHits(500, 600);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 35);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 40;

            Tamable = false;
            ControlSlots = 0;
        }

        public Glimmerjack(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 0;
        public override int Hides => 0;
        public override HideType HideType => HideType.Regular;
        public override FoodType FavoriteFood => FoodType.None;
        public override PackInstinct PackInstinct => PackInstinct.Daemon;
        public override bool CanFly => true;
        public override bool AlwaysAttackable => true;

        private void EndBlindness(object state)
        {
            if (state is Mobile mob)
                mob.SendLocalizedMessage(1019019); // You are no longer blinded.
        }

        public void ShimmeringNova(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 12) || !CanBeHarmful(target) || !InLOS(target))
                return;

            // Windup
            MovingParticles(target, 0x37CC, 1, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            // <-- fixed: pass an IEntity (EffectItem), not an EffectLayer -->
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                0x37CC, 1, 0, 9502, 6014, 0x11D, 0
            ); 

            PlaySound(0x22D);
            Say("*The Glimmerjack begins to hum with intense energy!*");

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (Deleted || target == null || target.Deleted || target.Map != Map)
                    return;

                PlaySound(0x22F);

                // <-- fixed here as well -->
                Effects.SendLocationParticles(
                    EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, 9502, 6014, 0x11D, 0
                );

                // Single-target burst
                target.FixedParticles(0x37C0, 10, 20, 0x11D, EffectLayer.CenterFeet);

                // AOE
                var list = new List<Mobile>();
                var nearby = Map.GetMobilesInRange(target.Location, 5);
                foreach (Mobile m in nearby)
                    if (m != this && CanBeHarmful(m, false))
                        list.Add(m);
                nearby.Free();

                foreach (var mob in list)
                {
                    DoHarmful(mob);

                    int rawDamage = Utility.RandomMinMax(30, 50);
                    int energyRes = mob.EnergyResistance;
                    int reduced = (int)(rawDamage * (1.0 - energyRes / 100.0));
                    reduced = Math.Max(1, reduced);

                    AOS.Damage(mob, this, reduced, 0, 0, 0, 0, 100);

                    if (Utility.RandomDouble() < 0.25)
                    {
                        if (Utility.RandomBool())
                        {
                            mob.SendLocalizedMessage(1019018);
                            mob.FixedParticles(0x376A, 10, 15, 5012, EffectLayer.Head);
                            mob.PlaySound(0x1D1);
                            Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(EndBlindness), mob);
                        }
                        else
                        {
                            int drain = Utility.RandomMinMax(20, 30);
                            if (mob is PlayerMobile pm)
                            {
                                pm.Mana -= drain;
                                pm.SendMessage("You feel your energy being drained!");
                            }
                            else if (mob is BaseCreature bc)
                            {
                                bc.Mana -= drain;
                            }
                            mob.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Head);
                            mob.PlaySound(0x28E);
                        }
                    }
                }

                _NextNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            });
        }

        public void DoTeleport()
        {
/*             if (Deleted || Map == null || Map == Map.Internal)
                return;

            var combatant = Combatant as Mobile;
            if (combatant == null || !CanBeHarmful(combatant))
                return;

            Point3D newLoc = Point3D.Zero;
            int attempts = 20, range = 8, minRange = 3;

            for (int i = 0; i < attempts; i++)
            {
                var p = new Point3D(
                    combatant.X + Utility.RandomMinMax(-range, range),
                    combatant.Y + Utility.RandomMinMax(-range, range),
                    0
                );

                // <-- fixed: use Utility.GetDistance instead of p.GetDistanceTo -->
                if (Map.CanSpawnMobile(p) && p.GetDistanceToSqrt(combatant.Location) >= minRange)
                {
                    newLoc = p;
                    break;
                }
            }

            if (newLoc == Point3D.Zero)
            {
                range = 10;
                for (int i = 0; i < attempts; i++)
                {
                    var p = new Point3D(
                        X + Utility.RandomMinMax(-range, range),
                        Y + Utility.RandomMinMax(-range, range),
                        0
                    );

                    if (Map.CanSpawnMobile(p))
                    {
                        newLoc = p;
                        break;
                    }
                }
            }

            if (newLoc != Point3D.Zero)
            {
                Say("*The Glimmerjack phases out!*");
				Effects.SendLocationParticles(
					EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
					0x37CC, 1, 0, 9502, 6014, 0x11D, 0, 0
				);

                Effects.PlaySound(Location, Map, 0x1FE);

                MoveToWorld(newLoc, Map);

				Effects.SendLocationParticles(
					EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
					0x3709, 10, 30, 9502, 6014, 0x11D, 0, 0
				);

                Effects.PlaySound(Location, Map, 0x1FE);

                _NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
            } */
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (DateTime.UtcNow >= _NextNova)
            {
                var t = Combatant as Mobile;
                if (t != null && !t.Deleted && t.Map == Map && InRange(t, 12) && CanBeHarmful(t) && InLOS(t))
                    ShimmeringNova(t);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
