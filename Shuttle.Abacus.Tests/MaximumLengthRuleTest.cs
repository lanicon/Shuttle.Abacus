/*
    This file forms part of Shuttle.Abacus.

    Shuttle.Abacus - A constraint-based calculation engine.
    Copyright (C) 2016  Eben Roux

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using NUnit.Framework;
using Shuttle.Abacus;
using Shuttle.Abacus.Localisation;

namespace Abacus.Test.Unit
{
    [TestFixture]
    public class MaximumLengthRuleTest 
    {
        private const string value = "blah-blah";

        [Test]
        public void Should_fail_for_a_string_that_is_too_long()
        {
            const int length = 5;

            var rule = new MaximumLengthRule(length);

            Assert.IsTrue(rule.IsBrokenBy(value));

            Assert.AreEqual(string.Format(Resources.MaximumLengthRule, length), rule.Message.Text);
        }

        [Test]
        public void Should_pass_for_a_string_that_is_short_enough()
        {
            Assert.IsFalse(new MaximumLengthRule(10).IsBrokenBy(value));
        }
    }
}
