using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a fencreep corpse")]
    public class Fencreep : BaseCreature
    {
        private DateTime _NextBogBlast;
        private DateTime _BogBlastWindupEnd;
        private bool     _IsWindingUpBogBlast;

        private DateTime _NextVineGrasp;
        private DateTime _NextCorrosiveSecretion;
        private DateTime _NextSummonBoglings;

        [Constructable]
        public Fencreep()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name        = "a fencreep";
            Body        = 779;
            BaseSoundID = 422;
            Hue         = 0x8A3;

            SetStr(300, 400);
            SetDex(70,  90);
            SetInt(100, 150);

            SetHits(500, 600);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison,   50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,      20, 30);
            SetResistance(ResistanceType.Cold,      30, 40);
            SetResistance(ResistanceType.Poison,    60, 70);
            SetResistance(ResistanceType.Energy,    30, 40);

            SetSkill(SkillName.MagicResist, 90.1, 105.0);
            SetSkill(SkillName.Tactics,     85.1, 100.0);
            SetSkill(SkillName.Wrestling,   90.1, 105.0);
            SetSkill(SkillName.Magery,      80.0,  95.0);

            Fame       =  8000;
            Karma      = -8000;
            VirtualArmor = 40;

            _NextBogBlast           = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _NextVineGrasp          = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5,  10));
            _NextCorrosiveSecretion = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            _NextSummonBoglings     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
        }

        public Fencreep(Serial serial)
            : base(serial)
        {
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map ||
                !InRange(combatant, 18) || !CanBeHarmful(combatant))
                return;

            // Bog Blast
            if (!_IsWindingUpBogBlast && DateTime.UtcNow >= _NextBogBlast)
            {
                StartBogBlastWindup(combatant);
            }
            else if (_IsWindingUpBogBlast && DateTime.UtcNow >= _BogBlastWindupEnd)
            {
                ExecuteBogBlast(combatant);
            }

            // Other abilities (when not winding up)
            if (!_IsWindingUpBogBlast)
            {
                if (DateTime.UtcNow >= _NextVineGrasp && InRange(combatant, 3) && InLOS(combatant))
                {
                    VineGrasp(combatant);
                    _NextVineGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                }

                if (DateTime.UtcNow >= _NextSummonBoglings && InRange(combatant, 10))
                {
                    SummonBoglings();
                    _NextSummonBoglings = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 90));
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)  // 30% chance
                CorrosiveSecretions(defender);
        }

        public void StartBogBlastWindup(Mobile target)
        {
            if (target == null) return;

            _IsWindingUpBogBlast = true;
            _BogBlastWindupEnd    = DateTime.UtcNow + TimeSpan.FromSeconds(3.0);

            // Animate signature requires 6 parameters: action, frames, repeats, forward, repeatAnim, delay
            Animate((int)AnimationType.Spell, 7, 1, true, false, 10);

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*The fencreep gurgles and swells with noxious gas!*");
            PlaySound(BaseSoundID + 1);
        }

        public void ExecuteBogBlast(Mobile target)
        {
            _IsWindingUpBogBlast = false;
            _NextBogBlast        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            PlaySound(0x237);
            Effects.SendLocationEffect(Location, Map, 0x36B0, 16, 10, 0x8A3, 0);

            var victims = new List<Mobile>();
            foreach (var m in GetMobilesInRange(5))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m.Player || (m is BaseCreature bc && (bc.Controlled || bc.Summoned) && bc.FightMode == FightMode.Aggressor))
                    victims.Add(m);
            }

            foreach (var m in victims)
            {
                DoHarmful(m);

                // Poison damage
                int raw    = Utility.RandomMinMax(30, 50);
                int actual = (int)(raw * (1.0 - (m.PoisonResistance / 100.0)));
                AOS.Damage(m, this, Math.Max(1, actual), 0, 0, 0, 100, 0);

                if (m.Alive && Utility.RandomDouble() < 0.75)
                    m.ApplyPoison(this, Poison.Deadly);

                if (m.Alive && Utility.RandomDouble() < 0.5)
                {
                    m.Stam = Math.Max(0, m.Stam - Utility.RandomMinMax(10, 20));
                    if (m is PlayerMobile pm)
                        pm.SendAsciiMessage("The noxious gas drains your stamina!");
                }

                if (m is PlayerMobile pm2)
                    pm2.SendAsciiMessage("You are caught in the fencreep's bog blast!");
            }
        }

        public void VineGrasp(Mobile target)
        {
            if (target == null || !target.Alive || target.Stam <= 0 || target.Blessed)
                return;

            DoHarmful(target);

            target.PlaySound(0x1E3);
            // **FIXED**: added final “unknown” parameter (0)
            Effects.SendTargetParticles(
                target,
                0x3735,   // itemID
                1,        // speed
                30,       // duration
                0x8A3,    // hue
                0,        // renderMode
                9502,     // effect
                EffectLayer.Head,
                0         // unknown
            );

            if (!target.Paralyzed)
            {
                target.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)));
                if (target is PlayerMobile pm)
                    pm.SendAsciiMessage("Vines erupt from the ground, ensnaring you!");
            }
        }

        public void CorrosiveSecretions(Mobile target)
        {
            if (target == null || !target.Alive || target.Blessed)
                return;

            DoHarmful(target);

            target.PlaySound(0x21);
            // **FIXED**: added final “unknown” parameter (0)
            Effects.SendTargetParticles(
                target,
                0x374A,   // itemID
                1,        // speed
                15,       // duration
                0x8A3,    // hue
                0,        // renderMode
                9502,     // effect
                EffectLayer.Waist,
                0         // unknown
            );

            if (target is Mobile m)
            {
                var physMod   = new ResistanceMod(ResistanceType.Physical, -10);
                var poisonMod = new ResistanceMod(ResistanceType.Poison,   -15);
                m.AddResistanceMod(physMod);
                m.AddResistanceMod(poisonMod);

                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                {
                    m.RemoveResistanceMod(physMod);
                    m.RemoveResistanceMod(poisonMod);
                    if (m is PlayerMobile pm)
                        pm.SendAsciiMessage("The corrosive effect fades.");
                });

                if (m is PlayerMobile pm2)
                    pm2.SendAsciiMessage("You are covered in corrosive secretions!");
            }
        }

        public void SummonBoglings()
        {
            int existing = 0;
            foreach (var m in GetMobilesInRange(15))
            {
                if (m is Bogling bog && !bog.Controlled && !bog.Summoned)
                    existing++;
            }

            const int max = 3;
            if (existing >= max) return;

            int toSummon = Utility.RandomMinMax(1, max - existing);
            for (int i = 0; i < toSummon; i++)
            {
                var p = GetSpawnLocation(10);
                if (p != Point3D.Zero)
                {
                    var b = new Bogling();
                    b.MoveToWorld(p, Map);
                    b.Combatant = Combatant;
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*The fencreep stirs the muck, calling forth guardians!*");
                }
            }
        }

        private Point3D GetSpawnLocation(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                if (Map.CanSpawnMobile(x, y, z))
                    return new Point3D(x, y, z);
            }
            return Point3D.Zero;
        }

        public override int Hides => 12;
        public override int Meat  => 3;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel     => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);
            AddLoot(LootPack.FilthyRich);

            if (Utility.RandomDouble() < 0.05)
            {
                // PackItem(new SwampEssence());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(_NextBogBlast);
            writer.Write(_BogBlastWindupEnd);
            writer.Write(_IsWindingUpBogBlast);
            writer.Write(_NextVineGrasp);
            writer.Write(_NextCorrosiveSecretion);
            writer.Write(_NextSummonBoglings);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _NextBogBlast           = reader.ReadDateTime();
            _BogBlastWindupEnd      = reader.ReadDateTime();
            _IsWindingUpBogBlast    = reader.ReadBool();
            _NextVineGrasp          = reader.ReadDateTime();
            _NextCorrosiveSecretion = reader.ReadDateTime();
            _NextSummonBoglings     = reader.ReadDateTime();
        }
    }
}
