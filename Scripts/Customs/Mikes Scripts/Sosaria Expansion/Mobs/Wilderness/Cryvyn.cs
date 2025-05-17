using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cryvyn corpse")]
    public class Cryvyn : BaseCreature
    {
        private DateTime _NextArcticBlastUse;
        private Timer _AuraTimer;

        [Constructable]
        public Cryvyn() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cryvyn";
            Body = Utility.RandomList(60, 61);
            BaseSoundID = 362;

            Hue = 0x8B0;

            SetStr(1000, 1200);
            SetDex(200, 250);
            SetInt(300, 400);

            SetHits(1500, 2000);
            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold,     60);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     90,100);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   50, 60);

            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics,      130.0, 150.0);
            SetSkill(SkillName.Wrestling,    130.0, 150.0);
            SetSkill(SkillName.DetectHidden,  70.0,  80.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 75;

            Tamable = false;

            PackItem(new Gold(500, 1000));

            SetWeaponAbility(WeaponAbility.Dismount);
            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich,   4);
            AddLoot(LootPack.HighScrolls, 2);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new Sapphire());

            if (Utility.RandomDouble() < 0.50)
                PackItem(new WhiteScales(Utility.RandomMinMax(5, 15)));
        }

        private static readonly TimeSpan ArcticBlastCooldown = TimeSpan.FromSeconds(20.0);

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();
            InitiateArcticBlast();
        }

        private void InitiateArcticBlast()
        {
            if (DateTime.UtcNow < _NextArcticBlastUse || Combatant == null || Deleted || !Alive)
                return;

            var target = Combatant as Mobile;
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 15) || !CanBeHarmful(target) || !InLOS(target))
                return;

            // Wind‑up visuals
            MovingParticles(target, 0x3779, 10, 0, false, false, 0, 0, 9502, 6014, 0x1F3, EffectLayer.Head, 0);
            PlaySound(0x64F);
            PublicOverheadMessage(MessageType.Regular, Hue, false, "Cryvyn begins to gather immense cold!");

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () => DoArcticBlast(target));
            _NextArcticBlastUse = DateTime.UtcNow + ArcticBlastCooldown;
        }

        private void DoArcticBlast(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != Map || !CanBeHarmful(target))
                return;

            // <-- Fixed: pass Location then Map -->
/* 			Effects.SendLocationParticles(
				target.Location,
				target.Map,
				0x3709, // particle ID
				30,     // speed
				10,     // duration
				0x1F3,  // hue
				0,      // render mode
				EffectLayer.Head // or whatever layer you want
			); */
            Effects.PlaySound(target.Location, target.Map, 0x20E);

            var list = target.Map.GetMobilesInRange(target.Location, 8);
            foreach (Mobile m in list)
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                DoHarmful(m);

                int baseDamage        = Utility.RandomMinMax(40, 60);
                double coldMult       = 1.0 - (m.ColdResistance    / 100.0);
                double physMult       = 1.0 - (m.PhysicalResistance / 100.0);
                int    coldDamage     = (int)(baseDamage * 0.75 * coldMult);
                int    physDamage     = (int)(baseDamage * 0.25 * physMult);
                int    totalDamage    = Math.Max(1, coldDamage + physDamage);

                AOS.Damage(m, this, totalDamage, 25, 75, 0, 0, 0);

                // 30% chance to apply a brief slow
                if (Utility.RandomDouble() < 0.30 && m is Mobile mobileTarget)
                {
                    mobileTarget.SendLocalizedMessage(1008111, false, Name);
                    // simple Dex‐based slow instead of a Buff
                    mobileTarget.AddStatMod(new StatMod(
                        StatType.Dex,
                        "CryvynFreeze",
                        -20,                           // flat Dex reduction
                        TimeSpan.FromSeconds(5.0)
                    ));
                }
            }
            list.Free();
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            StartAuraTimer();
        }

        private void StartAuraTimer()
        {
            if (_AuraTimer == null || !_AuraTimer.Running)
                _AuraTimer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(3.0), new TimerCallback(ApplyAuraEffect));
        }

        private void StopAuraTimer()
        {
            _AuraTimer?.Stop();
        }

        private void ApplyAuraEffect()
        {
            if (Deleted || !Alive)
            {
                StopAuraTimer();
                return;
            }

            var list = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in list)
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                DoHarmful(m);
                int auraDamage = Utility.RandomMinMax(5, 10);
                AOS.Damage(m, this, auraDamage, 0, 100, 0, 0, 0);
                m.SendLocalizedMessage(1008111, false, Name);
            }
            list.Free();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25 && defender is Mobile mobileDefender)
            {
                mobileDefender.SendAsciiMessage("Cryvyn's touch chills you to the bone!");

                // delayed‐damage instead of ApplyDelayedDamage
                Timer.DelayCall(TimeSpan.FromMilliseconds(300), () =>
                    AOS.Damage(mobileDefender, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0)
                );

                // flat Dex slow instead of a missing buff class
                mobileDefender.AddStatMod(new StatMod(
                    StatType.Dex,
                    "CryvynChill",
                    -15,
                    TimeSpan.FromSeconds(6.0)
                ));
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && Utility.RandomDouble() < 0.10)
            {
                from.FixedParticles(0x374A, 1, 15, 9502, 0x1F3, 0, EffectLayer.Waist);
                from.PlaySound(0x025);
                from.SendAsciiMessage("Cryvyn emits a burst of cold in retaliation!");
                AOS.Damage(from, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            StopAuraTimer();
        }

        public override bool CanAngerOnTame    => false;
        public override bool ReacquireOnMovement => !Controlled;
        public override int TreasureMapLevel   => 5;
        public override int Meat               => 15;
        public override int Hides              => 30;
        public override HideType HideType      => HideType.Horned;
        public override int DragonBlood        => 15;
        public override FoodType FavoriteFood  => FoodType.Fish;

        public Cryvyn(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(_NextArcticBlastUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
            if (version >= 1)
                _NextArcticBlastUse = reader.ReadDateTime();
            StartAuraTimer();
        }
    }
}
