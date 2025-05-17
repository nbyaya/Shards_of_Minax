using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a graventh corpse")]
    public class Graventh : BaseCreature
    {
        // AOE attack properties
        private DateTime _NextGustAttack;
        private TimeSpan GustCooldown = TimeSpan.FromSeconds(15.0 + (5.0 * Utility.RandomDouble())); // 15–20s
        private TimeSpan GustWindupDuration = TimeSpan.FromSeconds(3.0);
        private DateTime _GustWindupStart;
        private bool _IsGustWindingUp = false;

        [Constructable]
        public Graventh()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a graventh";
            Body = 5;
            BaseSoundID = 0x2EE;
            Hue = 1175;

            SetStr(200, 250);
            SetDex(150, 200);
            SetInt(80, 120);

            SetHits(800, 1000);
            SetMana(0);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, 70.1, 90.0);
            SetSkill(SkillName.EvalInt, 70.1, 90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 40;

            Tamable = false;
            ControlSlots = 0;
            MinTameSkill = 0.0;
        }

        public Graventh(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Combatant.Deleted || Combatant.Map != Map || !InRange(Combatant, 12) || !CanBeHarmful(Combatant) || !InLOS(Combatant))
            {
                _IsGustWindingUp = false;
                return;
            }

            if (DateTime.UtcNow >= _NextGustAttack && !_IsGustWindingUp)
            {
                StartGustWindup();
            }
            else if (_IsGustWindingUp && DateTime.UtcNow >= _GustWindupStart + GustWindupDuration)
            {
                PerformGustAttack();
                _IsGustWindingUp = false;
                _NextGustAttack = DateTime.UtcNow + GustCooldown;
            }
        }

        public void StartGustWindup()
        {
            if (Combatant is Mobile target)
            {
                target.SendAsciiMessage("The graventh begins to gather powerful winds!");
                PlaySound(0x17C);
                Animate(AnimationType.Attack, 5);

                _GustWindupStart = DateTime.UtcNow;
                _IsGustWindingUp = true;
            }
        }

        public void PerformGustAttack()
        {
            if (!(Combatant is Mobile target))
                return;

            target.SendAsciiMessage("The graventh unleashes a devastating gust!");
            PlaySound(0x650);

            // — Updated SendLocationParticles to the 8‑argument overload —
			Effects.SendLocationParticles(
				EffectItem.Create(target.Location, target.Map, TimeSpan.FromSeconds(0.25)),
				Utility.RandomList(0x375A, 0x376A),
				30,
				10,
				5052,
				0,
				9502,
				0
			);


            int aoeRange = 4;
            var targetsInRange = new List<Mobile>();

            foreach (var mob in target.GetMobilesInRange(aoeRange))
            {
                if (mob != this && CanBeHarmful(mob))
                    targetsInRange.Add(mob);
            }

            foreach (var mob in targetsInRange)
            {
                DoHarmful(mob);

                int raw = Utility.RandomMinMax(30, 50);
                double eDmg = raw * 0.5 * (1.0 - (mob.EnergyResistance / 100.0));
                double pDmg = raw * 0.5 * (1.0 - (mob.PhysicalResistance / 100.0));
                int total = Math.Max(1, (int)(eDmg + pDmg));

                AOS.Damage(mob, this, total, 0, 0, 0, 0, 100);

                if (Utility.RandomDouble() < 0.75)
                {
                    mob.SendAsciiMessage("The gust disorients you, affecting your combat prowess!");

                    // — Updated SendTargetParticles to the 7‑argument overload —
                    Effects.SendTargetParticles(
                        mob,
                        0x37CB, // itemID
                        1,      // speed
                        20,     // duration
                        9502,   // hue
                        0,      // render mode
                        9910    // effect ID
                    );
                }
            }
        }

        private DateTime _NextScreech;
        private TimeSpan ScreechCooldown = TimeSpan.FromSeconds(7.0 + (3.0 * Utility.RandomDouble()));

        public void ScreechAttack(Mobile m)
        {
            if (m == null || m.Deleted || m.Map != Map || !InRange(m, 2) || !CanBeHarmful(m) || !InLOS(m))
                return;

            DoHarmful(m);
            PlaySound(0x5B9);

            Effects.SendTargetParticles(
                m,
                0x37CC,
                1,
                40,
                9502,
                0,
                9910
            );

            int raw = Utility.RandomMinMax(10, 15);
            AOS.Damage(m, this, raw, 50, 0, 0, 50, 0);

            if (Utility.RandomDouble() < 0.6 && m is PlayerMobile pm && pm.Spell != null)
            {
                // — Replaced pm.Casting with the ServUO Spell property —
                pm.Spell.OnCasterHurt();
            }

            _NextScreech = DateTime.UtcNow + ScreechCooldown;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.UtcNow >= _NextScreech && Utility.RandomDouble() < 0.33)
            {
                ScreechAttack(defender);
            }
        }

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 4;
        public override int Meat => 2;
        public override MeatType MeatType => MeatType.Bird;
        public override int Feathers => 50;
        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
        public override bool CanFly => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.HighScrolls);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new Skullveil());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(_NextGustAttack);
            writer.Write(_GustWindupStart);
            writer.Write(_IsGustWindingUp);
            writer.Write(_NextScreech);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                _NextGustAttack    = reader.ReadDateTime();
                _GustWindupStart   = reader.ReadDateTime();
                _IsGustWindingUp   = reader.ReadBool();
                _NextScreech       = reader.ReadDateTime();
            }

            if (_NextGustAttack == DateTime.MinValue)
                _NextGustAttack = DateTime.UtcNow + GustCooldown;
            if (_NextScreech == DateTime.MinValue)
                _NextScreech = DateTime.UtcNow + ScreechCooldown;
        }
    }
}
